local Component = require('component')
local class = require('classlib')

local stackWidgetComponent = class('stackWidgetComponent', Component)

function stackWidgetComponent:__init()

  local entity = self:getEntity(true)

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

  entity.drawOrder = 0

end

function stackWidgetComponent:eventEnabledChanged(args)
  if not args then return end

  if args.enabled then
    if args.component == self or
      args.system == self:getSystem() or
      args.entity == self:getEntity() or
      self:hasItem(args.component) then
      self:layoutItems()
    end
  end

  itemChanged(self, args)
end

function stackWidgetComponent:eventUpdate(args)
  updateKeys(self)
end

function stackWidgetComponent:eventItemChanged(args)
  if not args then return end

  for _, item in pairs(self.items) do
    if item == args.item then
      self:layoutItems()
      break
    end
  end

end

function stackWidgetComponent:addItems(...)
  for _, item in pairs({...}) do
    self:addItem(item)
  end
end

function stackWidgetComponent:addItem(item)
  assert(item and classname(item) and item.x and item.y and item.width and item.height,
		"Cannot add item because it is not considered a UI drawable object")
  table.insert(self.items, item)
  self:dispatchEvent("eventItemAdded", {widget = self, item = item})
  self:layoutItems()
end

function stackWidgetComponent:removeItem(item)
  for k,v in pairs(self.items) do
    if v == item then
      self.items[k] = nil
      self:dispatchEvent("eventItemRemoved", {widget = self, item = item})
      break
    end
  end
  self:reorderItems()
end

function stackWidgetComponent:hasItem(item)
  for k,v in pairs(self.items) do
    if v == item then
      return true
    end
  end
  return false
end

function stackWidgetComponent:setItemsEnabled(enable)
  assert(type(enable) == "boolean", "Must enter a boolean to setEnable")
  for _, item in pairs(self.items) do
    item:setEnabled(enable)
  end
end

function stackWidgetComponent:reorderItems()
  local newItems = {}
  for _, v in pairs(self.items) do
    table.insert(newItems, v)
  end

  self.items = newItems
  self:layoutItems()
end

function stackWidgetComponent:alignVerticalPosition()
  if self.verticalAlignment == "top" then
    self.y = self.verticalPadding
  elseif self.verticalAlignment == "bottom" then
    self.y = love.graphics.getHeight() - self.height - self.verticalPadding
  elseif self.verticalAlignment == "center" then
    self.y = (love.graphics.getHeight() - self.height) * 0.5
  end
end

function stackWidgetComponent:alignHorizontalPosition()
  if self.horizontalAlignment == "left" then
    self.x = self.horizontalPadding
  elseif self.horizontalAlignment == "right" then
    self.x = love.graphics.getWidth() - self.width - self.horizontalPadding
  elseif self.horizontalAlignment == "center" then
    self.x = (love.graphics.getWidth() - self.width) * 0.5
  end
end

function stackWidgetComponent:alignPosition()
  self:alignVerticalPosition()
  self:alignHorizontalPosition()
end

function stackWidgetComponent:calculateSize()
  self.width = 0
  self.height = 0
  if self.stackVertically then
    for i, item in ipairs(self.items) do
      if item:isEnabled() then
        self.width = math.max(self.width, item.width)
        self.height = self.height + self.space + item.height
      end
    end
  else
    for i, item in ipairs(self.items) do
      if item:isEnabled() then
        self.width = self.width + self.space + item.width
        self.height = math.max(self.height, item.height)
      end
    end
  end
end

function stackWidgetComponent:layoutItems()
  self:calculateSize()
  self:alignPosition()
  if self.stackVertically then self:layoutItemsVertically()
  else self:layoutItemsHorizontally() end
end

function stackWidgetComponent:layoutItemsVertically()
  local x, y, space = self.x, self.y, self.space
  self.width = 0
  for i, item in ipairs(self.items) do
    if item:isEnabled() then
      item.x = x
      item.y = y
      y = y + space + item.height
      self.width = math.max(self.width, item.width)
    end
  end
  self.height = y - self.y
end

function stackWidgetComponent:layoutItemsHorizontally()
  local x, y, space = self.x, self.y, self.space
  self.height = 0
  for i, item in ipairs(self.items) do
    if item:isEnabled() then
      item.x = x
      item.y = y
      x = x + space + item.width
      self.height = math.max(self.height, item.height)
    end
  end
  self.width = x - self.x
end

function stackWidgetComponent:eventResize(args)
  self:layoutItems()
end


local function widgetDraw(widget)
  local color = widget:getComponent("colorComponent")
  for _, item in ipairs(widget.items) do
    if item:isEnabled() then
      item:draw(color)
    end
  end
end

function stackWidgetComponent:eventDraw(args)
  if not args or args.drawOrder ~= self.drawOrder then return end

  if self.useScissor then
    love.graphics.setScissor(self.scissorX, self.scissorY,
                    self.scissorWidth, self.scissorHeight)
    widgetDraw(self)
    love.graphics.setScissor()
  else
    widgetDraw(self)
  end
end

return stackWidgetComponent
