local Component = require('component')
local class = require('classlib')
local ColorComponent = require("ColorComponent")

local SliderDrawerComponent = class('SliderDrawerComponent', Component)

function SliderDrawerComponent:__init(entity)
  self.Component:__init(entity, self)

  self.cursorColor = "gray"

  self.drawText = true
  self.drawBG = true
  self.drawCursor = true
  self.fontColor = "white"
  self.highlightColor = "yellow"
  self.pressedColor = "red"
  self.highlightScale = 1.1
  self.scaleX = 1
  self.scaleY = 1
  self.cursorScale = 0.1
end

function SliderDrawerComponent:eventDraw(args)
  local slider = self:getComponent("SliderComponent")
  if not slider then return end

  if self.drawBG then
    if slider.isMouseOver or slider.isClicked then
      local c = ColorComponent.getColor((slider.isClicked or slider.isClicked) and self.pressedColor or self.highlightColor)
      if c then
        love.graphics.setColor(c)
        local width, height = slider.width * self.highlightScale, slider.height * self.highlightScale
        love.graphics.rectangle("fill", slider.x - (width - slider.width)*0.5, slider.y - (height - slider.height)*0.5, width, height)
      end
    end
    local color = self:getComponent("ColorComponent")
    if color then
      love.graphics.setColor(color.r, color.g, color.b, color.a)
    end
    love.graphics.rectangle("fill", slider.x, slider.y, slider.width, slider.height)
  end

  if self.drawText then
    local c = ColorComponent.getColor(self.fontColor)
    if c then love.graphics.setColor(c) end
    love.graphics.printf(slider.text, slider.x, slider.y, slider.width, "center", 0, self.scaleX, self.scaleY)
  end

  if self.drawCursor then
    local color = ColorComponent.getColor(self.cursorColor)
    if color then
      love.graphics.setColor(color)
    end
    if slider.vertical then
      love.graphics.rectangle("fill", slider.x, slider.y + slider.height * slider:getPercentage(), slider.width, slider.height * self.cursorScale)
    else
      love.graphics.rectangle("fill", slider.x + slider.width * slider:getPercentage(), slider.y, slider.width * self.cursorScale, slider.height)
    end
  end
end

return SliderDrawerComponent
