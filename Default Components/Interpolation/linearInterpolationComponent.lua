local Component = require('component')
local class = require('classlib')

local linearInterpolationComponent = class('linearInterpolationComponent', Component, require('interpolationBase'))

function linearInterpolationComponent:__user_init(en)
  self:setDefault("name", classname(self))
  self:set("entity", en)

  local d =
  {
    start = 0,
    value = 0,
    speed = 1,
    linearInterpolationCurrent = 0,
  }
  d['end'] = 1
  en:setDefaultsAndValues(d)
end

function linearInterpolationComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local entity = self:getEntity()
  local start = entity.start
  local en = entity['end']
  local speed = entity.speed

  entity.linearInterpolationCurrent = ((entity.linearInterpolationCurrent + args.dt) * speed) % 1
  entity.value = start + (en - start) * entity.linearInterpolationCurrent

  for handler in pairs(self.interpolationBase.handlers) do
    handler(entity.value)
  end
end

lowerEventName(linearInterpolationComponent)

return linearInterpolationComponent
