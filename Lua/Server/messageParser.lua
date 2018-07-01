local Systems = require("systemList")

return function(l)
  local message = string.split(l, "|")
  local command = message[1]

  if command == "AddComponent" then
    local systemName = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local componentName = message[4]
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    entity:addComponent(componentName)

  elseif command == "AddEntity" then
    local systemName = message[2]
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    system:createEntity()

  elseif command == "BroadcastEvent" then
    Systems.broadcastEvent(message[2])

  elseif command == "DispatchEvent" then
    local systemName = message[2]
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    system:dispatchEvent(message[3])
    
  elseif command == "DispatchEventEntity" then
    local systemName = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    entity:dispatchEvent(message[4])

  elseif command == "Execute" then
    local f = assert(loadstring(message[2]))
    f()

  elseif command == "ReloadModule" then
    local modName = message[2]
    package.loaded[modName] = nil
    require(modName)

  elseif command == "RemoveComponent" then
    local systemName = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local componentID = assert(tonumber(message[4]), string.format("Cannot parse '%s' to number", message[4]))
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    assert(entity:removeComponent(componentID), "Component not removed")

  elseif command == "RemoveEntity" then
    local systemName = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    assert(entity:remove(), "Entity not removed")

  elseif command == "Reset" then
    love.event.quit("restart")

  elseif command == "SetComponentValue" then
    local systemName = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local componentID = assert(tonumber(message[4]), string.format("Cannot parse '%s' to number", message[4]))
    local key = assert(message[5], "Must have a key")
    local value = assert(message[6], "Must have a value")
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    local component = assert(entity:findComponent(componentID), string.format("Component with ID %d was not found", componentID))

    if tonumber(value) then
      component[key] = tonumber(value)
    elseif key == "enabled" then
      component:setEnabled(value:lower() == "true")
    elseif value:lower() == "true" then
      component[key] = true
    elseif value:upper() == "false" then
      component[key] = false
    else
      component[key] = value
    end

  elseif command == "SetEntityValue" then
    local systemName = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local key = message[4]
    local value = message[5]
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))

    if tonumber(value) then
      entity[key] = tonumber(value)
    elseif key == "enabled" then
      entity:setEnabled(value:lower() == "true")
    elseif value:lower() == "true" then
      entity[key] = true
    elseif value:upper() == "false" then
      entity[key] = false
    else
      entity[key] = value
    end

  elseif command == "SetSystemValue" then
    local systemName = message[2]
    local key = message[3]
    local value = message[4]
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))

    if tonumber(value) then
      system[key] = tonumber(value)
    elseif key == "enabled" then
      system:setEnabled(value:lower() == "true")
    elseif value:lower() == "true" then
      system[key] = true
    elseif value:upper() == "false" then
      system[key] = false
    else
      system[key] = value
    end

  else
    print(string.format("Unknown command '%s' received", command))
  end
end
