local Systems = {}

function Systems.addSystems(...)
  for _, s in pairs({...}) do
    table.insert(Systems, s)
  end
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
  for _, system in ipairs(Systems) do
    system:dispatchEvent('eventupdate', args)
  end
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
