local Component = require('component')
local class = require('classlib')

local coroutineComponent = class('coroutineComponent', Component)

function coroutineComponent:__init(en)
  self:setDefault('name', classname(self))
  self:set('entity', en)

  en:setDefaultsAndValues({
    current = 0,
    target = 0
  })
  self:setEnabled(false)
end

function coroutineComponent:setRoutine(routine)
  if self:isEnabled() then return end
  self:set("current", 0)
  self:set("target", 0)
  self:set("routine", coroutine.create(routine))
  self:setEnabled(true)
  return true
end

function coroutineComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local routine = self:get("routine")
  if not routine then return end

  local target = self:get("target") or 0
  if type(target) == "number" then
    local current = self:get("current")
    current = current + args.dt
    self:set("current", current)
    if current < target then return end
  elseif type(target) == "coroutineComponent" then
    if target:isEnabled() then return end
  end

  local status, rval = coroutine.resume(routine)
  if status then
    if type(rval) == "number" or type(rval) == "nil" then
      self:set("target", rval or 0)
      self:set("current", 0)
    elseif classname(rval) == "coroutineComponent" then
      self:set("target", rval)
    end
  else
    self:set("routine", nil)
    if rval ~= "cannot resume dead coroutine" then
      print(rval)
    end
  end
  self:setEnabled(status)
end

lowerEventName(coroutineComponent)

return coroutineComponent
