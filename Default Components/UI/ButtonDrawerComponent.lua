local Component = require('component')
local class = require('classlib')
local colorComponent = require("colorComponent")
local buttonDrawerComponent = class('buttonDrawerComponent', Component)

function buttonDrawerComponent:__init(entity)
  self.Component:__init(entity, self)

  self.drawText = true
  self.drawBG = true
  self.fontColor = "white"
  self.highlightColor = "yellow"
  self.pressedColor = "red"
  self.highlightScale = 1.1
  self.scaleX = 1
  self.scaleY = 1
  self.alignment = "center"
end

function buttonDrawerComponent:eventDraw(args)
  local button = self:getComponent("buttonComponent")
  if not button then return end

  if self.drawBG then
    if button.isMouseOver then
      button:drawHighlight(self.highlightScale, self.pressedColor, self.highlightColor)
    end
    button:drawButton()
  end

  if self.drawText then
    button:drawText(colorComponent.getColor(self.fontColor), self.alignment, self.scaleX, self.scaleY)
  end
end

return buttonDrawerComponent
