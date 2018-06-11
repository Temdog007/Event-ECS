local Component = require('component')
local class = require('classlib')

local StackWidgetComponent = class('StackWidgetComponent', Component)

function StackWidgetComponent:__init(entity)
  self.Component:__init(entity, self)
  self.items = {}
  self.x = 0
  self.y = 0
  self.width = 0
  self.height = 0
  self.space = 10

  self.sx = 0
  self.sy = 0
  self.autoSize = true
  self.vertical = true
  self.currentInterval = 0
  self.updateInterval = 1
end

function StackWidgetComponent:addItem(item)
  if not item or not classname(item) or
    not item.x or not item.y or
    not item.width or not item.height then
    error("Cannot add item because it is not considered a UI drawable object")
  end

  table.insert(self.items, item)
  self:dispatchEvent("eventItemAdded", {widget = self, item = item})
  self:layoutItems()
end

function StackWidgetComponent:removeItem(item)
  for k,v in pairs(self.items) do
    if v == item then
      self.items[k] = nil
      self:dispatchEvent("eventItemRemoved", {widget = self, item = item})
      break
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

function StackWidgetComponent:layoutItems()
  if self.vertical then self:layoutItemsVertically()
  else self:layoutItemsHorizontally() end
end

function StackWidgetComponent:layoutItemsVertically()
  local x, y, space = self.x, self.y, self.space
  if self.autoSize then
    self.width = 0
  end
  for i, item in ipairs(self.items) do
    if item:isEnabled() then
      item.x = x
      item.y = y
      y = y + space + item.height
      if self.autoSize then
        self.width = math.max(self.width, item.width)
      end
    end
  end
  if self.autoSize then
    self.height = y - self.y
  end
end

function StackWidgetComponent:layoutItemsHorizontally()
  local x, y, space = self.x, self.y, self.space
  if self.autoSize then
    self.height = 0
  end
  for i, item in ipairs(self.items) do
    if item:isEnabled() then
      item.x = x
      item.y = y
      x = x + space + item.width
      if self.autoSize then
        self.height = math.max(self.height, item.height)
      end
    end
  end
  if self.autoSize then
    self.width = x - self.x
  end
end

function StackWidgetComponent:eventUpdate(args)
  if not args or not args.dt then return end

  self.currentInterval = self.currentInterval + args.dt
  if self.currentInterval > self.updateInterval then
    self:layoutItems()
    self.currentInterval = 0
  end
end

function StackWidgetComponent:eventDraw(args)
  local color = self:getComponent("ColorComponent")

  if not self.autoSize then
    love.graphics.setScissor(self.sx, self.sy, self.width, self.height)
  end
  for i, item in ipairs(self.items) do
    if item:isEnabled() then
      item:draw(color)
    end
  end
  love.graphics.setScissor()
end

return StackWidgetComponent
