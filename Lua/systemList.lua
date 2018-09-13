require("tableExtensions")

local Systems = {}
local SystemMT = {}
local EventQueue = table.newqueue()

function SystemMT.addSystem(sys)
  table.insert(Systems, sys)
  return sys
end

local function insertSystem(t, i)
  i = i or 1
  if t[i] ~= nil then
    return SystemMT.addSystem(t[i]), insertSystem(t, i + 1)
  end
end

function SystemMT.addSystems(...)
  return insertSystem({...})
end

function SystemMT.removeAllSystems()
  Systems = {}
end

function SystemMT.removeSystem(system)
  assert(is_a(system, require("system")) or type(system) == "number", "Must enter a system or an id number to 'removeSystem.'")
  local newSystems = {}
  local removed = false
  for _, curSystem in pairs(Systems) do
    if curSystem == system or curSystem:getID() == system then
      removed = true
    else
      table.insert(newSystems, curSystem)
    end
  end
  if removed then
    Systems = newSystems
  end
  return removed
end

function SystemMT.hasEvent(eventName, args)
  for ev in table.queuepairs(EventQueue) do
    if ev[1] == eventName and ev[2] == args then
      return true
    end
  end
  return false
end

function SystemMT.pushEvent(eventName, args)
  SystemMT.pushEventRight(eventName, args)
end

function SystemMT.pushEventRight(eventName, args)
  table.pushright(EventQueue, {eventName, args})
end

function SystemMT.pushEventLeft(eventName, args)
  table.pushleft(EventQueue, {eventName, args})
end

function SystemMT.flushEvents()
  if #Systems == 0 then return end

  local size = table.queuesize(EventQueue)
  if size == 0 then return end
  local handled = 0

  local event, name, args
  while handled < size and table.queuesize(EventQueue) > 0 do
    event = table.popleft(EventQueue)
    name, args = event[1], event[2]
    for _, system in ipairs(Systems) do
      if system:isEnabled() or (args and args.ignoreEnabled) then
        system:dispatchEvent(name, args)
      end
    end
    handled = handled + 1
  end
end

function SystemMT.getSystem(value)
  for _, system in ipairs(Systems) do
    if system:getID() == tonumber(value) or system:getName() == value then
      return system
    end
  end
end

local function runOnSystem(t, i, f, ...)
  i = i or 1
  if t[i] ~= nil then
    return f(t[i], ...), runOnSystem(t, i + 1, f, ...)
  end
end

function SystemMT.forEachSystem(func, ...)
  return runOnSystem(Systems, 1, func, ...)
end

function SystemMT.getCount()
  return #Systems
end

SystemMT.__index = SystemMT
return setmetatable({}, SystemMT)
