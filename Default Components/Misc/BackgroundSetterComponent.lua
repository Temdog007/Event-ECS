local Component = require('component')
local class = require('classlib')

local BackgroundSetterComponent = class('BackgroundSetterComponent', Component)

function BackgroundSetterComponent:__init(entity)
  self.Component:__init(entity, self)
end

function BackgroundSetterComponent:eventUpdate(args)
  local color = self:getComponent("ColorComponent")
  if not color then return end

  love.graphics.setBackgroundColor(color.r, color.g, color.b, color.a)
  Log(string.format("Background setter removed: %s", tostring(self:remove())))
end

return BackgroundSetterComponent
