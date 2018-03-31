return function(constructor)
  Cls = {}
  Cls.__index = Cls

  return setmetatable(Cls,
  {
      __call = function(s, ...)
        local self = setmetatable({}, s)
        constructor(s, ...)
        return self
      end
  })
end
