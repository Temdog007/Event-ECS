local class = require('classlib')
local coroutineScheduler = class('coroutineScheduler')

function coroutineScheduler:__user_init()
  self.routines = {}
end

function coroutineScheduler:addRoutine(routine)
  assert(type(routine) == "function", "Must enter a function for a coroutine")

  table.insert(self.routines,
  {
    current = 0,
    target = 0,
    func = routine,
    routine = coroutine.create(routine)
  })
end

function coroutineScheduler:finishRoutine(routine)
  for _, value in pairs(self.routines) do
    if value.func == routine then
      local stauts
      repeat status = coroutine.resume(value.routine) until not status
      break
    end
  end
end

function coroutineScheduler:cancelRoutine(routine)
  for i, value in pairs(self.routines) do
    if value.func == routine then
      self.routines[i] = nil
      break
    end
  end
end

function coroutineScheduler:hasRoutine(routine)
  for _, value in pairs(self.routines) do
    if value.func == routine then return true end
  end
end

function coroutineScheduler:getStatus(routine)
  for _, value in pairs(self.routines) do
    if value.func == routine then
      return coroutine.status(value.routine)
    end
  end
end

function coroutineScheduler:clear()
  self.routines = {}
end

function coroutineScheduler:update(dt)
  for i, value in pairs(self.routines) do
    local routine = assert(value.routine, "No routine")
    local target = assert(value.target, "No target")
    if type(target) == "number" then
      local coroutineCurrent = assert(value.current, "No current")
      coroutineCurrent = coroutineCurrent + dt
      value.current = coroutineCurrent
      if coroutineCurrent < target then return end
    elseif classname(target) == "coroutineScheduler" then
      if coroutine.status(target.routine) == "running" then return end
    end

    local status, rval = coroutine.resume(routine)
    if status then
      if type(rval) == "number" or type(rval) == "nil" then
        value.target = rval or 0
        value.current = 0
      elseif classname(rval) == "coroutineScheduler" then
        value.target = rval
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
