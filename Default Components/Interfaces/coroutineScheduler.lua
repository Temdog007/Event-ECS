local class = require('classlib')
local coroutineScheduler = class('coroutineScheduler')

function coroutineScheduler:setRoutine(routine, override)
  if self.routine ~= nil and not override then return false end

  assert(type(routine) == "function", "Must enter a function for a coroutine")
  self.current = 0
  self.target = 0
  self.routine = coroutine.create(routine)
  return true
end

function coroutineScheduler:update(dt)
  local routine = self.routine
  if not routine then return end

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
    self.routine = nil
    if rval ~= "cannot resume dead coroutine" then
      print(rval)
    end
  end
end

return coroutineScheduler
