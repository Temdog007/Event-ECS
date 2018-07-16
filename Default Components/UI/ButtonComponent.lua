local Component = require('component')
local class = require('classlib')
local colorComponent = require("colorComponent")
local buttonComponent = class('buttonComponent', Component)

local defaultKeys = require('itemKeys')
local initKeys = defaultKeys.init
local updateKeys = defaultKeys.update

function buttonComponent:__init(entity)
  self.Component:__init(entity, self)

  self.text = ""
  self.isMouseOver = false
  self.isClicked = false
  self.x = 0
  self.y = 0
  self.width = 100
  self.height = 25

  initKeys(self)
end

function buttonComponent:isOver(x, y)
  assert(type(x) == "number" and type(y) == "number",
    string.format("Must enter numbers to 'isOver' %s %s", tostring(s), tostring(s)))
  return self.x < x and x < self.x + self.width and
          self.y < y and y < self.y + self.height
end

function buttonComponent:eventUpdate(args)
  updateKeys(self)
end

buttonComponent.eventItemsChanged = defaultKeys.itemChanged

function buttonComponent:eventMouseMoved(args)
  if not args then return end
  local x, y = args[1], args[2]
  if not x or not y then return end

  self.isMouseOver = self:isOver(x, y)
  if not self.isMouseOver then
    self.isClicked = false
  end
end

function buttonComponent:eventMousePressed(args)
  if not args then return end
  local x, y, b = args[1], args[2], args[3]
  if not x or not y or not b then return end

  if b == 1 and self:isOver(x, y) then
    self.isClicked = true
  end
end

function buttonComponent:eventMouseReleased(args)
  if not args then return end
  local b = args[3]
  if not b then return end

  if b == 1 then
    if self.isClicked and self.isMouseOver and self.action then
      self.action()
    end
    self.isClicked = false
  end
end

function buttonComponent:drawHighlight(scale, pressedColor, highlightColor)
  scale = scale or 1.1
  local c = colorComponent.getColor(self.isClicked and (pressedColor or "red") or (highlightColor or "yellow"))
  if c then
    love.graphics.setColor(c)
    local width, height = self.width * scale, self.height * scale
    love.graphics.rectangle("fill", self.x - (width - self.width)*0.5, self.y - (height - self.height)*0.5, width, height)
  end
end

function buttonComponent:drawButton(color)
  local color = color or self:getComponent("colorComponent")
  if color then
    love.graphics.setColor(color)
  end
  love.graphics.rectangle("fill", self.x, self.y, self.width, self.height)
end

function buttonComponent:drawText(color, alignment, scaleX, scaleY)
  if color then love.graphics.setColor(color) end
  love.graphics.printf(self.text, self.x, self.y, self.width, alignment or "center", 0, scaleX or 1, scaleY or 1)
end

function buttonComponent:draw(fontColor)
  if self.isMouseOver then
    self:drawHighlight()
  end
  self:drawButton()
  self:drawText(fontColor)
end

return buttonComponent
