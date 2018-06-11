local Component = require('component')
local class = require('classlib')

local ButtonActionComponent = class('ButtonActionComponent', Component)

function ButtonActionComponent:__init(entity)
  self.Component:__init(entity, self)
end

function ButtonActionComponent:eventButtonClicked(args)
  if not args then return end

  local button = args.button
  if not button then return end

  if button ~= self:getComponent("ButtonComponent") then
    return
  end

  if self.action then
    self.action()
  elseif Log then
    Log("Button was clicked but an action has not been defined")
  end
end

return ButtonActionComponent
