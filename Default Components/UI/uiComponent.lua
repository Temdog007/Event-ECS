local Component = require('component')
local class = require('classlib')
local uiComponent = class('uiComponent', Component, require("valueWatcher"))

function uiComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))

  local entity = self:getEntity(true)

  entity.x = 0
  entity.y = 0
  entity.highlightScaleX = 1.1
  entity.highlightScaleY = 1.1
  entity.width = 100
  entity.height = 100
  entity.isMouseOver = false
  entity.bgColor = {1,1,1,1}
  entity.pressedColor = {1,0,0,1}
  entity.highlightColor = {1,1,0,1}
  entity.drawingBg = true
  entity.drawingHighlight = true
  entity.lastValues = {}
  entity.drawOrder = 0
  entity.draw = uiComponent.draw

  entity.valueKeys = entity.valueKeys or {}
  entity.valueKeys.x = 0
  entity.valueKeys.y = 0
  entity.valueKeys.width = 100
  entity.valueKeys.height = 100

  local values = entity.values or {}
  values.x = true
  values.y = true
  values.highlightScaleX = true
  values.highlightScaleY = true
  values.width = true
  values.height = true
  values.isMouseOver = true
  values.bgColor = true
  values.pressedColor = true
  values.highlightColor = true
  values.drawingBg = true
  values.drawOrder = true
  values.drawingHighlight = true
  entity.values = values
end

function uiComponent:eventUpdate(args)
  self:update(self:getEntity())
end

function uiComponent:canDraw(args)
  local entity = self:getEntity()
  return args and args.drawOrder == entity.drawOrder
end

function uiComponent:eventMouseMoved(args)
  if not args then return end
  local x, y = args[1], args[2]
  if not x or not y then return end

  local entity = self:getEntity()
  entity.isMouseOver = self:isOver(x, y)
end

function uiComponent:isOver(x, y)
  local entity = self:getEntity()

  assert(type(x) == "number" and type(y) == "number",
    string.format("Must enter numbers to 'isOver' %s %s", tostring(s), tostring(s)))
  return entity.x < x and x < entity.x + entity.width and
          entity.y < y and y < entity.y + entity.height
end

function uiComponent:drawHighlight()
  local entity = self:getEntity()

  local c = entity.isClicked and entity.pressedColor or entity.highlightColor
  if c then
    love.graphics.setColor(c)
    local width, height = entity.width * entity.highlightScaleX, entity.height * entity.highlightScaleY
    love.graphics.rectangle("fill", entity.x - (width - entity.width)*0.5, entity.y - (height - entity.height)*0.5, width, height)
  end
end

function uiComponent:drawBg()
  local entity = self:getEntity()

  local color = entity.bgColor
  if color then love.graphics.setColor(color) end
  love.graphics.rectangle("fill", entity.x, entity.y, entity.width, entity.height)
end

function uiComponent:draw()
  local entity = self:getEntity()
  if entity.drawingHighlight and entity.isMouseOver then self:drawHighlight() end
  if entity.drawingBg then self:drawBg() end
end

lowerEventName(uiComponent)

return uiComponent
