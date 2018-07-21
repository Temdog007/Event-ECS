local Component = require('component')
local class = require('classlib')
local Colors = require("eventecscolors")

local SliderDrawerComponent = class('sliderDrawerComponent', Component)

function SliderDrawerComponent:__init()
  self:setDefaultsAndValues(
    "cursorColor", {Colors.getColor("gray")},
    "drawText", true,
    "drawBG", true,
    "drawCursor", true,
    "fontColor", {1,1,1,1},
    "highlightColor", {1,1,0,1},
    "pressedColor", {1,0,0,1},
    "alignment", "center",
    "highlightScale", 1.1,
    "scaleX", 1,
    "scaleY", 1,
    "cursorScale", 0.1,
    "drawOrder", 0)
end

function SliderDrawerComponent:eventDraw(args)
  if not args or args.drawOrder ~= self.drawOrder then return end

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
    local c = Colors.getColor(self.fontColor)
    slider:drawText(c, self.alignment, self.scaleX, self.scaleY)
  end

  if self.drawCursor then
    local color = Colors.getColor(self.cursorColor)
    slider:drawCursor(color, self.cursorScale)
  end
end

return SliderDrawerComponent
