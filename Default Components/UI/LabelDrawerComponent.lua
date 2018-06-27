local Component = require('component')
local class = require('classlib')

local labelDrawerComponent = class('labelDrawerComponent', Component)

function labelDrawerComponent:__init(entity)
  self.Component:__init(entity, self)
end

function labelDrawerComponent:eventDraw(args)
  local label = self:getComponent("labelComponent")
  if not label then return end

  local color = self:getComponent("colorComponent")
  label:draw(color)
end

return labelDrawerComponent
