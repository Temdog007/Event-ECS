local Component = require('component')
local class = require('classlib')
local colorComponent = require("colorComponent")

local SliderDrawerComponent = class('sliderDrawerComponent', Component)

function SliderDrawerComponent:__init(entity)
  self.Component:__init(entity, self)

  self.cursorColor = "gray"

  self.drawText = true
  self.drawBG = true
  self.drawCursor = true
  self.fontColor = "white"
  self.highlightColor = "yellow"
  self.pressedColor = "red"
  self.alignment = "center"
  self.highlightScale = 1.1
  self.scaleX = 1
  self.scaleY = 1
  self.cursorScale = 0.1
end

function SliderDrawerComponent:eventDraw(args)
  local slider = self:getComponent("sliderComponent")
  if not slider then return end

  if self.drawBG then
    if slider.isMouseOver or slider.isClicked then
      slider:drawHighlight(self.highlightScale, self.pressedColor, self.highlightColor)
    end
    local color = self:getComponent("colorComponent")
    slider:drawSlider(color)
  end

  if self.drawText then
    local c = colorComponent.getColor(self.fontColor)
    slider:drawText(c, self.alignment, self.scaleX, self.scaleY)
  end

  if self.drawCursor then
    local color = colorComponent.getColor(self.cursorColor)
    slider:drawCursor(color, self.cursorScale)
  end
end

return SliderDrawerComponent
