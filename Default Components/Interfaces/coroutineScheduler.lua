local class = require('classlib')
local coroutineScheduler = class('coroutineScheduler')

function coroutineScheduler:__user_init()
  self.routines = {}
  self.current = 0
  self.target = 0
end

function coroutineScheduler:setRoutine(routine, override)
  assert(type(routine) == "function", "Must enter a function for a coroutine")

  if #self.routines > 0 then
    if override then
      self.routines = {}
    else
      return
    end
  end

  table.insert(self.routines, coroutine.create(routine))
end

function coroutineScheduler:addRoutine(routine)
  assert(type(routine) == "function", "Must enter a function for a coroutine")

  table.insert(self.routines, coroutine.create(routine))
  return true
end

function coroutineScheduler:update(dt)
  for i, routine in pairs(self.routines) do

    local target = self.target
    if type(target) == "number" then
      local coroutineCurrent = self.current
      coroutineCurrent = coroutineCurrent + dt
      self.current = coroutineCurrent
      if coroutineCurrent < target then return end
    elseif classname(target) == "coroutineScheduler" then
      if coroutine.status(target.routine) == "running" then return end
    end

    local status, rval = coroutine.resume(routine)
    if status then
      if type(rval) == "number" or type(rval) == "nil" then
        self.target = rval or 0
        self.current = 0
      elseif classname(rval) == "coroutineScheduler" then
        self.target = rval
      end
    else
      self.routines[i] = nil
      if rval ~= "cannot resume dead coroutine" then
        print(rval)
      end
    end
  end
end

return coroutineScheduler
