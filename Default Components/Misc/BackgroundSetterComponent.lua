local Component = require('component')
local class = require('classlib')

local backgroundSetterComponent = class('backgroundSetterComponent', Component)

function backgroundSetterComponent:__init(entity)
  self.Component:__init(entity, self)
end

function backgroundSetterComponent:eventUpdate(args)
  local color = self:getComponent("colorComponent")
  if not color then return end

  love.graphics.setBackgroundColor(color)
  print(string.format("Background setter removed: %s", tostring(self:remove())))
end

return backgroundSetterComponent
