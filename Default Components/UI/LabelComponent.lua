local Component = require('component')
local class = require('classlib')

local LabelComponent = class('LabelComponent', Component)

function LabelComponent:__init(entity)
  self.Component:__init(entity, self)

  self.x = 0
  self.y = 0
  self.rotation = 0
  self.scaleX = 1
  self.scaleY = 1
  self.width = 100
  self.height = love.graphics.getFont():getHeight()
  self.text = ""
end

function LabelComponent:draw(color)
  if color then
    love.graphics.setColor(color)
  end
  love.graphics.printf(self.text, self.x, self.y, self.width, "center", self.rotation, self.scaleX, self.scaleY)
end

return LabelComponent
