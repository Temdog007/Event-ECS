local Component = require('component')
local class = require('classlib')

local ButtonActionComponent = class('ButtonActionComponent', Component)

function ButtonActionComponent:__init(entity)
  self.Component:__init(entity, self)
end

function ButtonActionComponent:eventMouseReleased(args)
  if not self.action then return end

  local button = self:getComponent("ButtonComponent")
  if not button then return end

  if button.isMouseOver then
    self.action()
  end
end

return ButtonActionComponent
