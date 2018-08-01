local class = require('classlib')

local interpolationBase = class('interpolationBase')

function interpolationBase:__user_init(en)
  self.handlers = {}
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
