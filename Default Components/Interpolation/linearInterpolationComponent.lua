local Component = require('component')
local class = require('classlib')

local linearInterpolationComponent = class('linearInterpolationComponent', Component, require('interpolationBase'))

function linearInterpolationComponent:__user_init(en)
  self:setDefault("name", classname(self))
  self:set("entity", en)

  local d =
  {
    linearInterpolationStart = 0,
    linearInterpolationEnd = 1,
    linearInterpolationValue = 0,
    linearInterpolationSpeed = 1,
    linearInterpolationCurrent = 0,
  }
  en:setDefaultsAndValues(d)
end

function linearInterpolationComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local entity = self:getData()
  entity.linearInterpolationCurrent = ((entity.linearInterpolationCurrent + args.dt) * entity.linearInterpolationSpeed) % 1
  entity.linearInterpolationValue =
    self:apply(
    entity.linearInterpolationStart,
    entity.linearInterpolationEnd,
    entity.linearInterpolationCurrent)

  for handler in pairs(self.interpolationBase.handlers) do
    handler(entity.linearInterpolationValue)
  end
end

lowerEventName(linearInterpolationComponent)

return linearInterpolationComponent
