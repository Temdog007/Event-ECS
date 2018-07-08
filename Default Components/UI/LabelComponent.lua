local Component = require('component')
local class = require('classlib')

local labelComponent = class('labelComponent', Component)

function labelComponent:__init(entity)
  self.Component:__init(entity, self)

  self.text = ""
  self.x = 0
  self.y = 0
  self.width = 100
  self.alignment = "center"
  self.height = love.graphics.getFont():getHeight()
  self.rotation = 0
  self.scaleX = 1
  self.scaleY = 1
end

function labelComponent:draw(color)
  if color then
    love.graphics.setColor(color)
  end
  love.graphics.printf(self.text, self.x, self.y, self.width, self.alignment,
   self.rotation, self.scaleX, self.scaleY)
end

return labelComponent
