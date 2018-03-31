local Class = require("classFactory")
local Entity = require("entity")
local ClassName = "Entity Component System"

return Class(function(system)
  local entities = {}

  function system:createEntity()
    local entity = Entity(self)
    table.insert(entities, entity)
    self:dispatchEvent("createdEntity", {entity = entity, system = self})
    return entity
  end

  function system:__tostring()
    return ClassName
  end

  function system:removeEntity(entity)
    for k, en in pairs(entities) do
      if en == entity then
        self:dispatchEvent("removingEntity", {entity = entity, system = self})
        entities[k] = nil
        return true
      end
    end
  end

  function system:removeEntities(matchFunction)
    for k, en in pairs(entities) do
      if matchFunction(en) then
        self:dispatchEvent("removingEntity", {entity = en, system = self})
        entities[k] = nil
      end
    end
  end

  function system:findEntity(findFunc)
    for _, en in pairs(entities) do
      if findFunc(en) then
        return en
      end
    end
  end

  function system:findEntities(findFunc)
    local tab = {}
    for _, en in pairs(entities) do
      if findFunc(en) then
        tab[#tab + 1] = en
      end
    end
    return tab
  end

  function system:dispatchEvent(event, args)
    for _, entity in pairs(entities) do
      entity:dispatchEvent(event, args)
    end
  end
end)
