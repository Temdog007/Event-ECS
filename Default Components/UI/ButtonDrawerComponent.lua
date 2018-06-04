local Component = require('component')
local class = require('classlib')
local ColorComponent = require("ColorComponent")
local ButtonDrawerComponent = class('ButtonDrawerComponent', Component)

function ButtonDrawerComponent:__init(entity)
  self.Component:__init(entity, self)

  self.drawText = true
  self.drawBG = true
  self.fontColor = "white"
  self.highlightColor = "yellow"
  self.pressedColor = "red"
  self.highlightScale = 1.1
  self.scaleX = 1
  self.scaleY = 1
end

function ButtonDrawerComponent:eventDraw(args)
  local button = self:getEntity().ButtonComponent
  if not button then return end

  if self.drawBG then
    if button.isMouseOver then
      local c = ColorComponent.getColor(button.isClicked and self.pressedColor or self.highlightColor)
      if c then
        love.graphics.setColor(c)
        local width, height = button.width * self.highlightScale, button.height * self.highlightScale
        love.graphics.rectangle("fill", button.x - (width - button.width)*0.5, button.y - (height - button.height)*0.5, width, height)
      end
    end
    local color = self:getComponent("ColorComponent")
    if color then
      love.graphics.setColor(color.r, color.g, color.b, color.a)
    end
    love.graphics.rectangle("fill", button.x, button.y, button.width, button.height)
  end

  if self.drawText then
    local c = ColorComponent.getColor(self.fontColor)
    if c then love.graphics.setColor(c) end
    love.graphics.printf(button.text, button.x, button.y, button.width, "center", 0, self.scaleX, self.scaleY)
  end
end

return ButtonDrawerComponent
