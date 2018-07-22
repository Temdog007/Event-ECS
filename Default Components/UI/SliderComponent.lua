local Component = require('uiComponent')
local class = require('classlib')
local sliderComponent = class('sliderComponent', Component)

function sliderComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))

  local entity = self:getEntity(true)

  entity.min = 0
  entity.max = 100
  entity.value = 0
  entity.text = ""
  entity.isClicked = false
  entity.vertical = false
  entity.scaleX = 1
  entity.scaleY = 1
  entity.cursorColor = {1,1,1,1}
  entity.fontColor = {1,1,1,1}
  entity.draw = {ui = self, draw = sliderComponent.draw}

  local values = entity.values or {}
  values.min = true
  values.max = true
  values.value = true
  values.text = true
  values.isClicked = true
  values.vertical = true
  values.scaleX = true
  values.scaleY = true
  values.cursorColor = true
  values.fontColor = true
  entity.values = values
end

function sliderComponent:updatePosition(x, y)
  local entity = self:getEntity(true)

  if entity.isClicked then
    local minX, maxX = self:getXBoundary()
    local minY, maxY = self:getYXBoundary()

    if entity.isMouseOver then
      if entity.vertical then
        entity.value = entity.min + (entity.max - entity.min) * (y - minY) / (maxY - minY)
      else
        entity.value = entity.min + (entity.max - entity.min) * (x - minX) / (maxX - minX)
      end
    else
      love.mouse.setPosition(math.min(maxX, math.max(minX, x)), math.min(maxY, math.max(minY, y)))
    end
  end
end

function sliderComponent:getXBoundary()
  local entity = self:getEntity(true)
  return entity.x, entity.x + entity.width
end

function sliderComponent:getYXBoundary()
  local entity = self:getEntity(true)
  return entity.y, entity.y + entity.height
end

function sliderComponent:getPercentage()
  local entity = self:getEntity(true)
  return (entity.value - entity.min) / (entity.max - entity.min)
end

function sliderComponent:eventMouseMoved(args)
  self.uiComponent:eventMouseMoved(args)
  self:updatePosition(x,y)

  local entity = self:getEntity()
  if entity.isClicked and entity.isMouseOver and entity.action then
    entity.action(entity.value, self:getPercentage())
  end
end

function sliderComponent:eventMousePressed(args)
  if not args then return end
  local x, y, b = args[1], args[2], args[3]
  if not x or not y or not b then return end

  if b == 1 and self:isOver(x, y) then
    local entity = self:getEntity()
    entity.isClicked = true
    self:updatePosition(x, y)
  end
end

function sliderComponent:eventMouseReleased(args)
  if not args then return end
  local b = args[3]
  if not b then return end

  if b == 1 then
    local entity = self:getEntity()
    entity.isClicked = false
  end
end

function sliderComponent:drawCursor()
  local entity = self:getEntity()

  local color = entity.cursorColor
  if color then love.graphics.setColor(color) end

  if entity.vertical then
    love.graphics.rectangle("fill", entity.x, entity.y + entity.height * self:getPercentage(), entity.width, entity.height * entity.scaleY)
  else
    love.graphics.rectangle("fill", entity.x + entity.width * self:getPercentage(), entity.y, entity.width * entity.scaleX, entity.height)
  end
end

function sliderComponent:drawText()
  local entity = self:getEntity()

  local color = entity.fontColor
  if color then love.graphics.setColor(color) end

  love.graphics.printf(entity.text, entity.x, entity.y, entity.width, entity.alignment, 0, entity.scaleX, entity.scaleY)
end

function sliderComponent:draw()
  local entity = self:getEntity()

  self.uiComponent:draw()

  if entity.drawingCursor then self:drawCursor() end
  if entity.drawingText then self:drawText() end
end

return sliderComponent
