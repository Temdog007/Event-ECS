local Component = require('component')
local class = require('classlib')

local StackWidgetComponent = class('StackWidgetComponent', Component)

function StackWidgetComponent:__init(entity)
  self.Component:__init(entity, self)
  self.items = {}
  self.x = 0
  self.y = 0
  self.scaleX = 1
  self.scaleY = 1
  self.width = 100
  self.height = 100
  self.space = 10

  self.useScissor = false
  self.sx = 0
  self.sy = 0
  self.sw = 100
  self.sh = 100

  self.autoUpdate = false
  self.vertical = true
  self.currentInterval = 0
  self.updateInterval = 1
end

function StackWidgetComponent:setItems(...)
  self.items = {}
  self:addItems(...)
end

function StackWidgetComponent:addItems(...)
  for _, item in pairs({...}) do
    if not item or not classname(item) or
      not item.x or not item.y or
      not item.width or not item.height or
      not item.scaleX or not item.scaleY then
      error("Cannot add item because it is not considered a UI drawable object")
    end

    table.insert(self.items, item)
    self:dispatchEvent("eventItemAdded", {widget = self, item = item})
  end
  self:layoutItems()
end

function StackWidgetComponent:removeItems(...)
  for _, item in pairs({...}) do
    for k,v in pairs(self.items) do
      if v == item then
        self.items[k] = nil
        self:dispatchEvent("eventItemRemoved", {widget = self, item = item})
        break
      end
    end
  end
  self:reorderItems()
end

function StackWidgetComponent:hasItem(item)
  for k,v in pairs(self.items) do
    if v == item then
      return true
    end
  end
  return false
end

function StackWidgetComponent:reorderItems()
  local newItems = {}
  for _, v in pairs(self.items) do
    table.insert(newItems, v)
  end

  self.items = newItems
  self:layoutItems()
end

function StackWidgetComponent:setItemsEnabled(value)
  for _, item in pairs(self.items) do
    item:setEnabled(value)
  end
end

function StackWidgetComponent:layoutItems()
  if self.vertical then self:layoutItemsVertically()
  else self:layoutItemsHorizontally() end
end

function StackWidgetComponent:layoutItemsVertically()
  local x, y, space = self.x, self.y, self.space
  self.width = 0
  for i, item in ipairs(self.items) do
    if item:isEnabled() then
      item.x = x
      item.y = y
      y = y + space + item.height * item.scaleY
      self.width = math.max(self.width, item.width)
      end
  end
  self.height = y - self.y
end

function StackWidgetComponent:layoutItemsHorizontally()
  local x, y, space = self.x, self.y, self.space
  self.height = 0
  for i, item in ipairs(self.items) do
    if item:isEnabled() then
      item.x = x
      item.y = y
      x = x + space + item.width * item.scaleX
      self.height = math.max(self.height, item.height)
    end
  end
  self.width = x - self.x
end

function StackWidgetComponent:eventUpdate(args)
  if not args or not args.dt then return end

  if self.autoUpdate then
    self.currentInterval = self.currentInterval + args.dt
    if self.currentInterval > self.updateInterval then
      self:layoutItems()
      self.currentInterval = 0
    end
  end
end

function StackWidgetComponent:eventDraw(args)
  local color = self:getComponent("ColorComponent")

  if self.useScissor then
    love.graphics.setScissor(self.sx, self.sy, self.sw, self.sh)
  end

  for i, item in ipairs(self.items) do
    if item:isEnabled() then
      item:draw(color)
    end
  end

  if self.useScissor then
    love.graphics.setScissor()
  end
end

function StackWidgetComponent:eventResize(args)
  self:layoutItems()
end

return StackWidgetComponent
