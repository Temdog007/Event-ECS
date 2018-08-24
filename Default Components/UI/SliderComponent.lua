local Component = require('uiComponent')
local class = require('classlib')
local sliderComponent = class('sliderComponent', Component)

function sliderComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))

  local entity = self:getData(true)

  entity.min = 0
  entity.max = 100
  entity.increment = 0.1
  entity.value = 0
  entity.text = ""
  entity.alignment = "center"
  entity.isClicked = false
  entity.vertical = false
  entity.drawingCursor = true
  entity.drawingText = true
  entity.cursorScale = 0.1
  entity.scaleX = 1
  entity.scaleY = 1
  entity.printValue = true
  entity.fitInBackground = true
  entity.round = false
  entity.cursorColor = {1,1,1,1}
  entity.fontColor = {1,1,1,1}
  entity.main = classname(self)

  local values = entity.values or {}
  values.min = true
  values.max = true
  values.value = true
  values.text = true
  values.isClicked = true
  values.vertical = true
  values.round = false
  values.cursorScale = true
  values.alignment = true
  values.scaleX = true
  values.scaleY = true
  values.cursorColor = true
  values.fitInBackground = true
  values.printValue = true
  values.fontColor = true
  values.drawingCursor = true
  values.drawingText = true
  entity.values = values
end

function sliderComponent:updatePosition(x, y)
  assert(tonumber(x) and tonumber(y), "Must update position with number")

  local entity = self:getData()

  if entity.isClicked then
    local minX, maxX = self:getXBoundary()
    local minY, maxY = self:getYXBoundary()

    if entity.isMouseOver then
      if entity.vertical then
        entity.value = entity.min + (entity.max - entity.min) * (y - minY) / (maxY - minY)
      else
        entity.value = entity.min + (entity.max - entity.min) * (x - minX) / (maxX - minX)
      end
      if entity.round then entity.value = math.floor(entity.value) end
    else
      love.mouse.setPosition(math.min(maxX, math.max(minX, x)), math.min(maxY, math.max(minY, y)))
    end
  end
end

function sliderComponent:getXBoundary()
  local entity = self:getData()
  return entity.x, entity.x + entity.width
end

function sliderComponent:getYXBoundary()
  local entity = self:getData()
  return entity.y, entity.y + entity.height
end

function sliderComponent:getPercentage()
  local entity = self:getData()
  return (entity.value - entity.min) / (entity.max - entity.min)
end

function sliderComponent:eventMouseMoved(args)
  self.uiComponent:eventMouseMoved(args)
  self:updatePosition(args[1], args[2])

  local entity = self:getData()
  if entity.isClicked and entity.isMouseOver and entity.action then
    entity.action(entity)
  end
end

function sliderComponent:eventMousePressed(args)

  if not args then return end
  local x, y, b = args[1], args[2], args[3]
  if not x or not y or not b then return end

  if b == 1 and self:isOver(x, y) then
    local entity = self:getData()
    entity.isClicked = true
    self:updatePosition(x, y)
    if entity.isMouseOver and entity.action then
      entity.action(entity)
    end
  end
end

function sliderComponent:eventMouseReleased(args)
  if not args then return end
  local b = args[3]
  if not b then return end

  if b == 1 then
    local entity = self:getData()
    entity.isClicked = false
  end
end

function sliderComponent:drawCursor()
  local entity = self:getData()

  local color = entity.cursorColor
  if color then love.graphics.setColor(color) end

  if entity.vertical then
    love.graphics.rectangle("fill", entity.x, entity.y + entity.height * self:getPercentage(), entity.width, entity.height * entity.cursorScale)
  else
    love.graphics.rectangle("fill", entity.x + entity.width * self:getPercentage(), entity.y, entity.width * entity.cursorScale, entity.height)
  end
end

function sliderComponent:drawText()
  local entity = self:getData()

  local color = entity.fontColor
  if color then love.graphics.setColor(color) end
  if entity.font then love.graphics.setFont(entity.font) end

  local text
  if entity.printValue then
    if string.len(entity.text) > 0 then
      text = string.format("%s: %2.2f", entity.text, entity.value)
    else
      text = string.format("%2.2f", entity.value)
    end
  else
    text = entity.text
  end

  if entity.fitInBackground then
    local font = love.graphics.getFont()
    local w = font:getWrap(text, entity.width)
    local h = font:getHeight()
    if w * entity.scaleX > entity.width or h * entity.scaleY > entity.height then
      love.graphics.printf(text, entity.x, entity.y, entity.width, entity.alignment, 0, entity.width / w, entity.height / h)
    else
      love.graphics.printf(text, entity.x, entity.y, entity.width, entity.alignment, 0, entity.scaleX, entity.scaleY)
    end
  else
    love.graphics.printf(text, entity.x, entity.y, entity.width, entity.alignment, 0, entity.scaleX, entity.scaleY)
  end
end

function sliderComponent:draw()
  local entity = self:getData()

  self.uiComponent:draw()

  if entity.drawingCursor then self:drawCursor() end
  if entity.drawingText then self:drawText() end
end

lowerEventName(sliderComponent)

return sliderComponent
