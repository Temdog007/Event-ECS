local Component = require('component')
local class = require('classlib')

local linearPingPongInterpolationComponent = class('linearPingPongInterpolationComponent', Component, require('interpolationBase'))

function linearPingPongInterpolationComponent:__user_init(en)
  self:setDefault("name", classname(self))
  self:set("entity", en)

  local d =
  {
    start = 0,
    value = 0,
    speed = 1,
    current = 0,
  }
  d['end'] = 1
  en:setDefaultsAndValues(d)
end

function linearPingPongInterpolationComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local entity = self:getEntity()
  local start = entity.start
  local en = entity['end']
  local speed = entity.speed

  entity.current = ((entity.current + args.dt) * speed) % 2
  if entity.current < 1 then
    entity.value = start + (en - start) * entity.current
  else
    local c = (entity.current - 1) / 1
    entity.value = en + (start - en) * c
  end

  for handler in pairs(self.interpolationBase.handlers) do
    handler(entity.value)
  end
end

lowerEventName(linearPingPongInterpolationComponent)

return linearPingPongInterpolationComponent
