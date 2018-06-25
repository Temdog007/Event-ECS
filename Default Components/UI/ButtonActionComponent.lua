local Component = require('component')
local class = require('classlib')

local ButtonActionComponent = class('ButtonActionComponent', Component)

function ButtonActionComponent:__init(entity)
  self.Component:__init(entity, self)
end

function ButtonActionComponent:eventMouseReleased(args)
  if not args then return end
  local x, y = args[1], args[2]
  if not x or not y then return end

  local button = self:getComponent("ButtonComponent")
  if not button then return end

  if button.isClicked and button:isOver(x, y) then
    if self.action then
      self.action()
    else
      print("Button was clicked but has not action defined")
    end
  end
end

return ButtonActionComponent
