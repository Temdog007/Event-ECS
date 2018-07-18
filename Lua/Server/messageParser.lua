local Systems = require("systemList")

local function parseFunction(l)
  assert(type(l) == "string", "Must pass a string to parse")
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
    Systems.pushEvent(message[2])

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
    assert(entity:removeComponent(componentID), "A component not removed")

  elseif command == "RemoveEntity" then
    local systemName = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))
    assert(entity:remove(), "Entity not removed")

  elseif command == "Reset" then
    love.event.quit("restart")

  elseif command == "SetEntityValue" then
    local systemName = message[2]
    local entityID = assert(tonumber(message[3]), string.format("Cannot parse '%s' to number", message[3]))
    local key = assert(message[4], "Must have a key")
    local value = assert(message[5], "Must have a value")
    local system = assert(Systems.getSystem(systemName), string.format("No system with the name '%s'", systemName))
    local entity = assert(system:findEntity(entityID), string.format("No entity with ID %d found", entityID))

    if tonumber(key) then key = tonumber(key) end
    
    if tonumber(value) then
      entity:set(key, tonumber(value))
    elseif key == "enabled" then
      entity:setEnabled(value:lower() == "true")
    elseif value:lower() == "true" then
      entity:set(key, true)
    elseif value:lower() == "false" then
      entity:set(key, false)
    else
      entity:set(key, value)
    end

  else
    print(string.format("Unknown command '%s' received", command))
  end
end

return setmetatable({},
{
  __call = function(s, l)
    return parseFunction(l)
  end
})
