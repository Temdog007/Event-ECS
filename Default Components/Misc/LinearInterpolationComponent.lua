local Component = require('component')
local class = require('classlib')

local LinearInterpolationComponent = class('LinearInterpolationComponent', Component)

function LinearInterpolationComponent:__init(entity)
  self.Component:__init(entity, self)

  self.start = 0
  self["end"] = 1
  self.value = 0
  self.speed = 1
  self.current = 0
  self.handlers = {}
end

function LinearInterpolationComponent:eventUpdate(args)
  if not args or not args.dt then return end

  self.current = (self.current + args.dt) % self.speed
  self.value = self.start + (self["end"] - self.start) * (self.current / self.speed)
  for _, handler in pairs(self.handlers) do
    handler(self.value)
  end
end

function LinearInterpolationComponent:addHandler(func)
  assert(type(func) == "function", "Handlers must a be function")
  table.insert(self.handlers, func)
end

function LinearInterpolationComponent:removeHandler(func)
  assert(type(func) == "function", "Handlers must a be function")
  for k, v in pairs(self.handlers) do
    if v == func then
      self.handlers[k] = nil
      return true
    end
  end
  return false
end

return LinearInterpolationComponent
