local Component = require('component')
local class = require('classlib')

local FPSDisplayerComponent = class('FPSDisplayerComponent', Component)

function FPSDisplayerComponent:__init(entity)
  self.Component:__init(entity, self)
  self.x = 0
  self.y = 0
  self.scaleX = 1
  self.scaleY = 1
  self.limit = 100
end

function FPSDisplayerComponent:eventDraw(args)
  local color = self:getComponent("ColorComponent")
  if color then
    love.graphics.setColor(color)
  end
  love.graphics.print(love.timer.getFPS(), self.x, self.y, 0, self.scaleX, self.scaleY)
end

return FPSDisplayerComponent
