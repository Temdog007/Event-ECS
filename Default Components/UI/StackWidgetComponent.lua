local Component = require('uiComponent')
local class = require('classlib')

local stackWidgetComponent = class('stackWidgetComponent', Component)

function stackWidgetComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))

  local entity = self:getEntity(true)

  entity.bgColor = {0,0,0,0}
  entity.pressedColor = {0,0,0,0}
  entity.highlightColor = {0,0,0,0}

  entity.items = {}
  entity.x = 0
  entity.y = 0
  entity.width = 0
  entity.height = 0
  entity.space = 10

  -- scissor
  entity.scissorX = 0
  entity.scissorY = 0
  entity.scissorWidth = 100
  entity.scissorHeight = 100
  entity.useScissor = false

  --alignment
  entity.verticalAlignment = "none"
  entity.verticalPadding = 0
  entity.horizontalAlignment = "none"
  entity.horizontalPadding = 0

  entity.stackVertically = true

  entity.valueKeys = entity.valueKeys or {}
  entity.valueKeys.verticalAlignment = true
  entity.valueKeys.verticalPadding = true
  entity.valueKeys.horizontalAlignment = true
  entity.valueKeys.horizontalPadding = true

  local values = entity.values or {}
  values.bgColor = true
  values.pressedColor = true
  values.highlightColor = true
  values.x = true
  values.y = true
  values.width = true
  values.height = true
  values.space = true
  values.scissorX = true
  values.scissorY = true
  values.scissorWidth = true
  values.scissorHeight = true
  values.useScissor = true
  values.verticalAlignment = true
  values.verticalPadding = true
  values.horizontalAlignment = true
  values.horizontalPadding = true
  values.stackVertically = true
  entity.values = values
end

function stackWidgetComponent:setEnabled(bool)
  self.ecsObject:setEnabled(bool)
  if self:isEnabled() then
    self:layoutItems()
  end
end

function stackWidgetComponent:eventItemChanged(args)
  self.valueWatcherComponent:eventItemChanged(args)
  self:layoutItems()
end

function stackWidgetComponent:addItems(...)
  for _, item in pairs({...}) do
    self:addItem(item)
  end
end

function stackWidgetComponent:addItem(item)
  assert(item and item:get("x") and item:get("y") and item:get("width") and item:get("height"),
		"Cannot add item because it is not considered a UI drawable object")
  table.insert(self:get("items"), item)
  self:dispatchEvent("eventItemAdded", {widget = self, item = item})
  self:layoutItems()
end

function stackWidgetComponent:removeItem(item)
  local items = self:get("items")
  for k,v in pairs(items) do
    if v == item then
      items[k] = nil
      self:dispatchEvent("eventItemRemoved", {widget = self, item = item})
      break
    end
  end
  self:reorderItems()
end

function stackWidgetComponent:hasItem(item)
  for k,v in pairs(self:get("items")) do
    if v == item or v:getID() == item then
      return true
    end
  end
  return false
end

function stackWidgetComponent:setItemsEnabled(enable)
  assert(type(enable) == "boolean", "Must enter a boolean to setEnable")
  for _, item in pairs(self:get("items")) do
    item:setEnabled(enable)
  end
end

function stackWidgetComponent:reorderItems()
  local newItems = {}
  for _, v in pairs(self:get("items")) do
    table.insert(newItems, v)
  end

  self:set("items", newItems)
  self:layoutItems()
end

function stackWidgetComponent:alignVerticalPosition()
  local entity = self:getEntity()
  if entity.verticalAlignment == "top" then
    entity.y = entity.verticalPadding
  elseif entity.verticalAlignment == "bottom" then
    entity.y = love.graphics.getHeight() - entity.height - entity.verticalPadding
  elseif entity.verticalAlignment == "center" then
    entity.y = (love.graphics.getHeight() - entity.height) * 0.5
  end
end

function stackWidgetComponent:alignHorizontalPosition()
  local entity = self:getEntity()
  if entity.horizontalAlignment == "left" then
    entity.x = self.horizontalPadding
  elseif entity.horizontalAlignment == "right" then
    entity.x = love.graphics.getWidth() - entity.width - entity.horizontalPadding
  elseif entity.horizontalAlignment == "center" then
    entity.x = (love.graphics.getWidth() - entity.width) * 0.5
  end
end

function stackWidgetComponent:alignPosition()
  self:alignVerticalPosition()
  self:alignHorizontalPosition()
end

function stackWidgetComponent:calculateSize()
  local entity = self:getEntity()
  entity.width = 0
  entity.height = 0
  if entity.stackVertically then
    for i, item in ipairs(entity.items) do
      if item:isEnabled() then
        entity.width = math.max(entity.width, item:get("width"))
        entity.height = entity.height + entity.space + item:get("height")
      end
    end
  else
    for i, item in ipairs(entity.items) do
      if item:isEnabled() then
        entity.width = entity.width + entity.space + item:get("width")
        entity.height = math.max(entity.height, item:get("height"))
      end
    end
  end
end

function stackWidgetComponent:layoutItems()
  self:calculateSize()
  self:alignPosition()
  if self:get("stackVertically") then self:layoutItemsVertically()
  else self:layoutItemsHorizontally() end
end

function stackWidgetComponent:layoutItemsVertically()
  local entity = self:getEntity()
  local x, y, space = entity.x, entity.y, entity.space
  entity.width = 0
  for i, item in ipairs(entity.items) do
    if item:isEnabled() then
      item:set("x", x)
      item:set("y", y)
      y = y + space + item:get("height")
      entity.width = math.max(entity.width, item:get("width"))
    end
  end
  entity.height = y - entity.y
end

function stackWidgetComponent:layoutItemsHorizontally()
  local entity = self:getEntity()
  local x, y, space = entity.x, entity.y, entity.space
  entity.height = 0
  for i, item in ipairs(entity.items) do
    if item:isEnabled() then
      item:set("x", x)
      item:set("y", y)
      x = x + space + item:get("width")
      entity.height = math.max(entity.height, item:get("height"))
    end
  end
  entity.width = x - entity.x
end

function stackWidgetComponent:eventResize(args)
  self:layoutItems()
end

local function widgetDraw(widget)
  for _, item in ipairs(widget.items) do
    if item:isEnabled() then
      local d = item:get("draw")
      d.draw(d.ui)
    end
  end
end

function stackWidgetComponent:eventDraw(args)
  if not self:canDraw(args) then return end

  local entity = self:getEntity()
  if entity.useScissor then
    love.graphics.setScissor(entity.scissorX, entity.scissorY,
                    entity.scissorWidth, entity.scissorHeight)
    widgetDraw(entity)
    love.graphics.setScissor()
  else
    widgetDraw(entity)
  end
end

lowerEventName(stackWidgetComponent)

return stackWidgetComponent
