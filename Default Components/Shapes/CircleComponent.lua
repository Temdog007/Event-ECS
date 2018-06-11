local Component = require('component')
local class = require('classlib')

local CircleComponent = class('CircleComponent', Component)

function CircleComponent:__init(entity)
  self.Component:__init(entity, self)

  self.mode = "fill"
  self.x = 0
  self.y = 0
  self.radius = 10
  self.segments = 8
end

function CircleComponent:eventDraw(args)
  local color = self:getComponent("ColorComponent")
  if color then love.graphics.setColor(color) end
  love.graphics.circle(self.mode, self.x, self.y, self.radius, self.segments)
end

return CircleComponent
