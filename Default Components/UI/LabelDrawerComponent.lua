local Component = require('component')
local class = require('classlib')

local LabelDrawerComponent = class('LabelDrawer', Component)

function LabelDrawerComponent:__init(entity)
  self.Component:__init(entity, self)
end

function LabelDrawerComponent:eventDraw(args)
  local label = self:getComponent("LabelComponent")
  if not label then return end

  local color = self:getComponent("ColorComponent")
  label:draw(color)
end

return LabelDrawerComponent
