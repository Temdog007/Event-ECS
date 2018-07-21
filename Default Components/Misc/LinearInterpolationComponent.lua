local Component = require('component')
local class = require('classlib')

local linearInterpolationComponent = class('linearInterpolationComponent', Component)

function linearInterpolationComponent:__init()
  local entity = self:getEntity()

  entity.base = self
  entity.start = 0
  entity["end"] = 1
  entity.value = 0
  entity.speed = 1
  entity.current = 0
  entity.handlers = {}

  local values = entity.values or {}
  values.start = true
  values["end"] = true
  values.value = true
  values.speed = true
  values.current = true
  entity.values = values
end

function linearInterpolationComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local entity = self:getEntity()
  entity.current = (entity.current + args.dt) % entity.speed
  entity.value = entity.start + (entity["end"] - entity.start) * (entity.current / entity.speed)
  for _, handler in pairs(entity.handlers) do
    handler(entity.value)
  end
end

function linearInterpolationComponent:addHandler(func)
  assert(type(func) == "function", "Handlers must a be function")
  local entity = self:getEntity()
  table.insert(entity.handlers, func)
end

function linearInterpolationComponent:removeHandler(func)
  assert(type(func) == "function", "Handlers must a be function")
  local entity = self:getEntity()
  for k, v in pairs(entity.handlers) do
    if v == func then
      entity.handlers[k] = nil
      return true
    end
  end
  return false
end

lowerEventName(linearInterpolationComponent)

return linearInterpolationComponent
