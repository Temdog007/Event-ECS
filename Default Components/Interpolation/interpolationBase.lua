local class = require('classlib')

local interpolationBase = class('interpolationBase')

function interpolationBase:__user_init(en)
  self.handlers = {}
end

function interpolationBase.interpolate(a)
  return a
end

function interpolationBase:apply(s, e, a, func)
  func = func or self.interpolate
  return s + (e - s) * func(a)
end

function interpolationBase:addHandler(func)
  assert(type(func) == "function", "Handlers must a be function")
  self.handlers[func] = true
end

function interpolationBase:removeHandler(func)
  assert(type(func) == "function", "Handlers must a be function")
  self.handlers[func] = nil
end

return interpolationBase
