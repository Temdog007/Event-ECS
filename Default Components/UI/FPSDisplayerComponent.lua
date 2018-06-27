local Component = require('component')
local class = require('classlib')

local fpsDisplayerComponent = class('fpsDisplayerComponent', Component)

function fpsDisplayerComponent:__init(entity)
  self.Component:__init(entity, self)
  self.x = 0
  self.y = 0
  self.scaleX = 1
  self.scaleY = 1
  self.limit = 100
end

function fpsDisplayerComponent:eventDraw(args)
  local color = self:getComponent("colorComponent")
  if color then
    love.graphics.setColor(color)
  end
  love.graphics.print(love.timer.getFPS(), self.x, self.y, 0, self.scaleX, self.scaleY)
end

return fpsDisplayerComponent
