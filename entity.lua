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

local json = require("json")
local ClassFactory = require("classFactory")
local ClassName = "Entity"

local function removeAll()
  return true
end

local function entostring(en)
  if en.name then
    return string.format("%s#%d: %s", ClassName, en:getID(), en.name)
  else
    return string.format("%s#%d", ClassName, en:getID())
  end
end

local Entity = ClassFactory(function(entity, system)
  assert(system, "Entity must have a system")

  function entity:getName()
    return entostring(self)
  end

  entity.system = system

  local components = {}
  local eventTablesTable = {}

  function entity:addComponent(compName, args)
    local compClass = assert(self.system:getComponent(compName), "Component not found in system")
    local component = assert(compClass(self, args), "Class instance couldn't be created")

    -- Insert component into list of components
    table.insert(components, component)

    -- Grab all of the components functions and put them into the event tables
    for k,v in pairs(compClass) do
      if type(v) == "function" then
        if not eventTablesTable[k] then
          eventTablesTable[k] = {}
        end
        table.insert(eventTablesTable[k], {component = component, func = v})
      end
    end

    self.system:dispatchEvent("addedComponent",  {component = component, entity = self})
    return component
  end

  local function addComponentsFromTable(s, t, i)
    i = i or 1
    if t[i] ~= nil then
      return s:addComponent(t[i]), addComponentsFromTable(s, t, i + 1)
    end
  end

  function entity:addComponents(...)
    return addComponentsFromTable(self, {...})
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
      return 0
    end

    local count = 0
    for _, eventTable in pairs(eventTables) do
      local comp = eventTable.component
      if comp:isEnabled() then
        eventTable.func(comp, args)
        count = count + 1
      end
    end
    return count
  end

  function entity:remove()
    self:removeComponents(removeAll)
    return self.system:removeEntity(self)
  end

  function entity:getData()
    local data = {}
    data.id = self:getID()
    for _, component in pairs(components) do
      for k, compValue in pairs(component) do
        if k ~= "__type" and (type(compValue) == "number" or type(compValue) == "string") then
          data[k] = compValue
        end
      end
    end
    return data
  end

  function entity:encode()
    return json.encode(self:getData())
  end

end)
return Entity
