local Component = require('component')
local class = require('classlib')

local buttonActionComponent = class('buttonActionComponent', Component)

function buttonActionComponent:__init(entity)
  self.Component:__init(entity, self)
end

function buttonActionComponent:eventMouseReleased(args)
  if not args then return end
  local x, y = args[1], args[2]
  if not x or not y then return end

  local button = self:getComponent("buttonComponent")
  if not button then return end

  if button.isClicked and button:isOver(x, y) then
    if self.action then
      self.action()
    else
      print("Button was clicked but has not action defined")
    end
  end
end

return buttonActionComponent
