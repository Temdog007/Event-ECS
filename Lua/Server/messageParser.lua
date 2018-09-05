local Systems = require("systemList")

local function parseFunction(l)
  assert(type(l) == "string", "Must pass a string to parse")
  local message = string.split(l, "|")
  local command = message[1]

  if command == "AddComponent" then
    local systemID = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local componentName = message[4]
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    entity:addComponent(componentName)

  elseif command == "AddEntity" then
    local systemID = message[2]
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))
    system:createEntity(message[3])

  elseif command == "BroadcastEvent" then
    Systems.pushEvent(message[2])

  elseif command == "DispatchEvent" then
    local systemID = message[2]
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))
    system:dispatchEvent(message[3])

  elseif command == "DispatchEventEntity" then
    local systemID = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    entity:dispatchEvent(message[4])

  elseif command == "Execute" then
    local f = assert(loadstring(message[2]))
    f()

  elseif command == "ReloadModule" then
    local modName = message[2]
    if package.loaded[modName] then
      package.loaded[modName] = nil
      print(string.format("Reloaded: '%s'", tostring(modName)))
    else
      print(string.format("Module '%s' was not loaded", tostring(modName)))
    end

  elseif command == "RemoveComponent" then
    local systemID = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local componentID = assert(tonumber(message[4]), string.format("Cannot parse '%s' to number", message[4]))
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    assert(entity:removeComponent(componentID), "A component not removed")

  elseif command == "RemoveEntity" then
    local systemID = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    assert(entity:remove(), "Entity not removed")

  elseif command == "Reset" then
    love.event.quit("restart")

  elseif command == "SetSystemValue" then
    local systemID = message[2]
    local key = assert(message[3], "Must have a key")
    local value = assert(message[4], "Must have a value")
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))

    if tonumber(key) then key = tonumber(key) end

    if tonumber(value) then
      system:set(key, tonumber(value))
    elseif key == "enabled" then
      system:setEnabled(value:lower() == "true")
    elseif value:lower() == "true" then
      system:set(key, true)
    elseif value:lower() == "false" then
      system:set(key, false)
    else
      system:set(key, value)
    end

  elseif command == "SetEntityValue" then
    local systemID = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local key = assert(message[4], "Must have a key")
    local value = assert(message[5], "Must have a value")
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))

    if string.starts(value, "{") and string.ends(value, "}") then
      local f, err = loadstring("return "..value)
      if not f then error(err) end
      entity:set(key, f())
    else
      if tonumber(key) then key = tonumber(key) end

      if tonumber(value) then
        entity:set(key, tonumber(value))
      elseif key == "enabled" then
        entity:setEnabled(value:lower() == "true")
        system:updateEnabledEntities()
      elseif value:lower() == "true" then
        entity:set(key, true)
      elseif value:lower() == "false" then
        entity:set(key, false)
      else
        entity:set(key, value)
      end
    end

  elseif command == "SetComponentEnabled" then
    local systemID = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local compID = assert(tonumber(message[4]), string.format("Cannot parse '%s' to number", message[4]))
    local value = assert(message[5], "Must have a value")
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    local comp = entity:findComponent(compID)
    comp:setEnabled(value:lower() == "true")

  elseif command == "SetSystemEnabled" then
    local systemID = message[2]
    local value = assert(message[3], "Must have a value")
    local system = assert(Systems.getSystem(systemID), string.format("No system with the ID %d found", systemID))
    system:setEnabled(value:lower() == "true")

  else
    error(string.format("Unknown command '%s' received", command))
  end
end

return setmetatable({},
{
  __call = function(s, l)
    return parseFunction(l)
  end
})
