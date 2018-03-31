local Class = require("classFactory")
local ClassName = "Entity"
local id = 0

local function removeAll()
  return true
end

local Entity = Class(function(entity, system)
  if not system then
    error("Entity must have a system")
  end

  entity.system = system

  local components = {}
  local eventTablesTable = {}
  local entityID = id
  entity.getID = function() return entityID end
  id = id + 1

  function entity:__tostring()
    return ClassName
  end

  function entity:addComponent(component)
    if not component then
      error("Tried to add a component that is nil")
    end
    if type(component) ~= "table" then
      error(string.format("Tried to add a '%s' as a component"))
    end

    -- Insert component into list of components
    table.insert(components, component)

    -- Grab all of the components functions and put them into the event tables
    for k,v in pairs(component) do
      if type(v) == "function" then
        if not eventTablesTable[k] then
          eventTablesTable[k] = {}
        end
        table.insert(eventTablesTable[k], {component = component, func = v})
      end
    end

    self.system:dispatchEvent("addedComponent",  {component = component, entity = self})
  end

  function entity:addComponents(...)
    for _, component in pairs({...}) do
      self:addComponent(component)
    end
  end

  local function removeComponentFunction(index, component)
    entity.system:dispatchEvent("removingComponent", {component = component, entity = entity})
    components[index] = nil
    for eventName,eventTables in pairs(eventTablesTable) do
      for k, eventTable in pairs(eventTables) do
        if eventTable.component == component then
          eventTable[k] = nil
        end
      end
    end
  end

  function entity:removeComponent(component)
    for k,v in pairs(components) do
      if v == component then
        removeComponentFunction(k, component)
        return true
      end
    end
  end

  function entity:removeComponents(compFunc)
    for k,component in pairs(components) do
      if compFunc(component) then
        removeComponentFunction(k, component)
      end
    end
  end

  function entity:componentCount()
    local count = 0
    for _ in pairs(components) do
      count = count + 1
    end
    return count
  end

  function entity:findComponent(matchFunction)
    for _, component in pairs(components) do
      if matchFunction(component) then
        return component
      end
    end
  end

  function entity:findComponents(matchFunction)
    local tab = {}
    for _, component in pairs(components) do
      if matchFunction(component) then
        tab[#tab + 1] = component
      end
    end
    return tab
  end

  function entity:dispatchEvent(event, args)
    local eventTables = eventTablesTable[event]
    if not eventTables then
      return
    end

    for _, eventTable in pairs(eventTables) do
      eventTable.func(args)
    end
  end

  function entity:remove()
    self:removeComponents(removeAll)
    return self.system:removeEntity(self)
  end

end)
return Entity
