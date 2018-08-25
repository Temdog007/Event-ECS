local Component = require('component')
local class = require('classlib')

local circleOutInterpolationComponent = class('circleOutInterpolationComponent', Component, require('interpolationBase'))

function circleOutInterpolationComponent:__init(entity)
  self:setDefault('name', classname(self))
  self:set('entity', entity)

  local d =
  {
    circleOutInterpolationStart = 0,
    circleOutInterpolationEnd = 1,
    circleOutInterpolationValue = 0,
    circleOutInterpolationSpeed = 1,
    circleOutInterpolationCurrent = 0,
  }
  en:setDefaultsAndValues(d)
end

function circleOutInterpolationComponent.interpolate(a)
  a = a - 1
  return math.sqrt(1 - a * a)
end

function circleOutInterpolationComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local entity = self:getData()
  entity.circleOutInterpolationCurrent = math.min(1, ((entity.circleOutInterpolationCurrent + args.dt) * entity.circleOutInterpolationSpeed) % 1)
  entity.circleOutInterpolationValue =
    self:apply(
    entity.circleOutInterpolationStart,
    entity.circleOutInterpolationEnd,
    entity.circleOutInterpolationCurrent,
    interpolate)

  Systems.pushEvent("eventcircleOutinterpolation", {value = entity.circleOutInterpolationValue, id = entity.id})
end

lowerEventName(circleOutInterpolationComponent)

return circleOutInterpolationComponent
