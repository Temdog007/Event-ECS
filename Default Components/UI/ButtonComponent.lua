local Component = require('component')
local class = require('classlib')
local ColorComponent = require("ColorComponent")
local ButtonComponent = class('ButtonComponent', Component)

function ButtonComponent:__init(entity)
  self.Component:__init(entity, self)

  self.text = ""
  self.isMouseOver = false
  self.isClicked = false
  self.x = 0
  self.y = 0
  self.alignment = "center"
  self.rotation = 0
  self.scaleX = 1
  self.scaleY = 1
  self.width = 100
  self.height = 25
end

function ButtonComponent:isOver(x, y)
  return self.x < x and x < self.x + self.width * self.scaleX and
          self.y < y and y < self.y + self.height * self.scaleY
end

function ButtonComponent:eventMouseMoved(args)
  if not args then return end
  local x, y = args[1], args[2]
  if not x or not y then return end

  self.isMouseOver = self:isOver(x, y)
  if not self.isMouseOver then
    self.isClicked = false
  end
end

function ButtonComponent:eventMousePressed(args)
  if not args then return end
  local x, y, b = args[1], args[2], args[3]
  if not x or not y or not b then return end

  if b == 1 and self:isOver(x, y) then
    self.isClicked = true
  end
end

function ButtonComponent:eventMouseReleased(args)
  if not args then return end
  local x, y, b = args[1], args[2], args[3]
  if not x or not y or not b then return end

  if b == 1 then
    if self.isClicked and self:isOver(x, y) then
      BroadcastEvent("eventbuttonclicked", {button = self})
    end
    self.isClicked = false
  end
end

function ButtonComponent:drawHighlight(scale, pressedColor, highlightColor)
  scale = scale or 1.1
  local c = ColorComponent.getColor(self.isClicked and (pressedColor or "red") or (highlightColor or "yellow"))
  if c then
    love.graphics.setColor(c)
    local width, height = self.width * scale, self.height * scale
    love.graphics.rectangle("fill", self.x - (width - self.width)*0.5, self.y - (height - self.height)*0.5, width, height)
  end
end

function ButtonComponent:drawButton(color)
  local color = color or self:getComponent("ColorComponent")
  if color then
    love.graphics.setColor(color)
  end
  love.graphics.rectangle("fill", self.x, self.y, self.width, self.height)
end

function ButtonComponent:drawText(color)
  if color then love.graphics.setColor(color) end
  love.graphics.printf(self.text, self.x, self.y, self.width, self.alignment, self.rotation, self.scaleX, self.scaleY)
end

function ButtonComponent:draw(color)

  if self.isMouseOver then
    self:drawHighlight()
  end
  self:drawButton()
  self:drawText(color)
end

return ButtonComponent
