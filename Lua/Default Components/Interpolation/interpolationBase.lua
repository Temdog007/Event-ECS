local class = require('classlib')

local interpolationBase = class('interpolationBase')

function interpolationBase:apply(s, e, a)
  assert(self.interpolation, "No interpolation function has been set")
  return s + (e - s) * self.interpolation(a)
end

return interpolationBase
