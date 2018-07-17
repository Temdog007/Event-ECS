local Component = require('component')
local class = require('classlib')
local Colors = require("eventecscolors")
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
  self.drawOrder = 0
end

function buttonDrawerComponent:eventDraw(args)
  if not args or args.drawOrder ~= self.drawOrder then return end

  local button = self:getComponent("buttonComponent")
  if not button then return end

  if self.drawBG then
    if button.isMouseOver then
      button:drawHighlight(self.highlightScale, self.pressedColor, self.highlightColor)
    end
    button:drawButton()
  end

  if self.drawText then
    button:drawText(Colors.getColor(self.fontColor), self.alignment, self.scaleX, self.scaleY)
  end
end

return buttonDrawerComponent
