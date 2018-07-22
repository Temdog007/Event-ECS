local Component = require('drawableComponent')
local class = require('classlib')
local buttonDrawerComponent = class('buttonDrawerComponent', Component)

function buttonDrawerComponent:__init()
  self:setDefault("name", classname(self))
end

function buttonDrawerComponent:eventDraw(args)
  local entity = self:getEntity()
  if not args or args.drawOrder ~= entity.drawOrder then return end

  local button = entity.buttonDrawerComponent
  if button then button:draw() end
end

return buttonDrawerComponent
