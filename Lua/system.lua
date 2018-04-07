-- Copyright (c) 2018 Temdog007
--
-- Permission is hereby granted, free of charge, to any person obtaining a copy
-- of this software and associated documentation files (the "Software"), to deal
-- in the Software without restriction, including without limitation the rights
-- to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
-- copies of the Software, and to permit persons to whom the Software is
-- furnished to do so, subject to the following conditions:
--
-- The above copyright notice and this permission notice shall be included in all
-- copies or substantial portions of the Software.
--
-- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
-- IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
-- FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
-- AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
-- LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
-- OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
-- SOFTWARE.

local ClassFactory = require("classFactory")
local Entity = require("entity")
local json = require("json")
local Component = require("component")
local ClassName = "Entity Component System"

local function systostring(sys)
  if sys.name then
    return string.format("%s: %s", ClassName, sys.name)
  else
    return ClassName
  end
end

return ClassFactory(function(system)
  local entities = setmetatable({},
  {
    __newindex = function(tab, key, value)
      value.getID = function() return key end
      rawset(tab, key, value)
    end
  })

  local registeredComponents = {}

  function system:getName()
    return systostring(self)
  end

  function system:createEntity()
    local entity = Entity(self)
    entities[#entities + 1] = entity
    self:dispatchEvent("eventCreatedEntity", {entity = entity, system = self})
    return entity
  end

  function system:removeEntity(entity)
    assert(entity and string.match(entity:getName(), "Entity"), "Must enter a entity to remove")
    for k, en in pairs(entities) do
      if en == entity then
        self:dispatchEvent("eventRemovingEntity", {entity = entity, system = self})
        entities[k] = nil
        return true
      end
    end
  end

  function system:removeEntities(matchFunction)
    assert(typeof(matchFunction) == "function", "Must enter a function to remove entities")

    local count = 0
    for k, en in pairs(entities) do
      if matchFunction(en) then
        self:dispatchEvent("eventRemovingEntity", {entity = en, system = self})
        entities[k] = nil
        count = count + 1
      end
    end
    return count
  end

  function system:entityCount()
    local count = 0
    for _ in pairs(entities) do
      count = count + 1
    end
    return count
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
    local eventsHandled = 0
    for _, entity in pairs(entities) do
      eventsHandled = eventsHandled + entity:dispatchEvent(event, args)
    end
    return eventsHandled
  end

  function system:registerComponent(NewComponent)
    local name = classname(NewComponent)
    assert(not registeredComponents[name], "Component has already been registered")
    assert(is_a(NewComponent, Component), 'Object is not an Component')
    registeredComponents[name] = NewComponent
  end
  system:registerComponent(Component)

  function system:registerComponents(...)
    for _, comp in pairs({...}) do
      self:registerComponent(comp)
    end
  end

  function system:getComponent(compName)
    assert(type(compName) == "string", "Must find components by classname")
    return registeredComponents[compName]
  end
end)
