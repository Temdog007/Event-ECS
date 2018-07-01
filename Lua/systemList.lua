local Systems = {}
local EventQueue = {}

function Systems.addSystem(sys)
  table.insert(Systems, sys)
  return sys
end

local function insertSystem(t, i)
  i = i or 1
  if t[i] ~= nil then
    return Systems.addSystem(t[i]), insertSystem(t, i + 1)
  end
end

function Systems.addSystems(...)
  return insertSystem({...})
end

function Systems.removeSystem(system)

  local newSystems = {}
  for _,s in ipairs(Systems) do
    if s ~= system then
      table.insert(newSystems, s)
    end
  end

  Systems = newSystems
end

function Systems.broadcastEvent(eventName, args)
  table.insert(EventQueue, {eventName, args})
end

function Systems.flushEvents()
  if #EventQueue == 0 or #Systems == 0 then return end

  for _, event in ipairs(EventQueue) do
    local eventName, eventArgs = event[1], event[2]
    for _, system in ipairs(Systems) do
      system:dispatchEvent(eventName, eventArgs)
    end
  end
  EventQueue = {}
end

function Systems.getSystem(name)
  for _, system in ipairs(Systems) do
    if system.name == name then
      return system
    end
  end
end

function Systems.forEachSystem(func, ...)
  for _, system in ipairs(Systems) do
    func(system, ...)
  end
end

return Systems
