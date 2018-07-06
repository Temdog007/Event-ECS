local Systems = {}
local SystemMT = {}
local EventQueue = {}

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

function SystemMT.removeSystem(system)

  local newSystems = {}
  for _,s in ipairs(Systems) do
    if s ~= system then
      table.insert(newSystems, s)
    end
  end

  Systems = newSystems
end

function SystemMT.pushEvent(eventName, args)
  table.insert(EventQueue, {eventName, args})
end

function SystemMT.flushEvents()
  if #EventQueue == 0 or #Systems == 0 then return end

  for i, event in ipairs(EventQueue) do
    local eventName, eventArgs = event[1], event[2]
    for _, system in ipairs(Systems) do
      system:dispatchEvent(eventName, eventArgs)
    end
    EventQueue[i] = nil
  end
end

function SystemMT.getSystem(name)
  for _, system in ipairs(Systems) do
    if system:getName() == name then
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
