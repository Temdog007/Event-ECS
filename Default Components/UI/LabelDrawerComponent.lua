local Component = require('component')
local class = require('classlib')

local labelDrawerComponent = class('labelDrawerComponent', Component)

function labelDrawerComponent:__init(entity)
  self.Component:__init(entity, self)

  self.drawOrder = 0
end

function labelDrawerComponent:eventDraw(args)
  if not args or args.drawOrder ~= self.drawOrder then return end

  local label = self:getComponent("labelComponent")
  if not label then return end

  local color = self:getComponent("colorComponent")
  label:draw(color)
end

return labelDrawerComponent
