local Component = require('component')
local class = require('classlib')
local Systems = require("systemList")
local linearPingPongInterpolationComponent = class('linearPingPongInterpolationComponent', Component, require('interpolationBase'))

function linearPingPongInterpolationComponent:__user_init(en)
  self:setDefault("name", classname(self))
  self:set("entity", en)

  local d =
  {
    linearPingPongInterpolationStart = 0,
    linearPingPongInterpolationEnd = 1,
    linearPingPongInterpolationValue = 0,
    linearPingPongInterpolationSpeed = 1,
    linearPingPongInterpolationCurrent = 0,
  }
  en:setDefaultsAndValues(d)
end

function linearPingPongInterpolationComponent.interpolate(a)
  if a < 1 then
    return a
  else
    return math.abs(1 - ((a - 1) / 1))
  end
end

function linearPingPongInterpolationComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local entity = self:getData()

  entity.linearPingPongInterpolationCurrent = math.min(2, (entity.linearPingPongInterpolationCurrent + (args.dt * entity.linearPingPongInterpolationSpeed)) % 2)
  entity.linearPingPongInterpolationValue =
    self:apply(entity.linearPingPongInterpolationStart,
      entity.linearPingPongInterpolationEnd,
      entity.linearPingPongInterpolationCurrent,
      linearPingPongInterpolationComponent.interpolate)

  Systems.pushEvent("eventlinearpingponginterpolation", {value = entity.linearPingPongInterpolationValue, id = entity.id})
end

lowerEventName(linearPingPongInterpolationComponent)

return linearPingPongInterpolationComponent
