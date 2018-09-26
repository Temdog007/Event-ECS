local Component = require('component')
local class = require('classlib')
local Systems = require("systemList")
local linearInterpolationComponent = class('linearInterpolationComponent', Component, require('interpolationBase'))

function linearInterpolationComponent.interpolate(a)
  return a
end

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
  self.interpolationBase.interpolation = linearInterpolationComponent.interpolate
end

function linearInterpolationComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local entity = self:getData()
  entity.linearInterpolationCurrent = math.min(1, (entity.linearInterpolationCurrent + (args.dt * entity.linearInterpolationSpeed)) % 1)
  entity.linearInterpolationValue =
    self:apply(
    entity.linearInterpolationStart,
    entity.linearInterpolationEnd,
    entity.linearInterpolationCurrent)

  Systems.pushEvent("eventlinearinterpolation", {value = entity.linearInterpolationValue, id = entity.id})
end

lowerEventName(linearInterpolationComponent)

return linearInterpolationComponent
