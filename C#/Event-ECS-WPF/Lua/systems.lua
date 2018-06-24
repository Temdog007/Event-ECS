local Systems = {}

function addSystem(system)
  table.insert(Systems, system)
end

function removeSystem(system)

  local newSystems = {}
  for _,s in ipairs(Systems) do
    if s ~= system then
      table.insert(newSystems, s)
    end
  end

  Systems = newSystems
end

function broadcastEvent(eventName, args)
  for _, system in ipairs(Systems) do
    system:dispatchEvent('eventupdate', args)
  end
end

function getSystem(name)
  for _, system in ipairs(Systems) do
    if system.name == name then
      return system
    end
  end
end

function forEachSystem(func, ...)
  for _, system in ipairs(Systems) do
    func(system, ...)
  end
end
