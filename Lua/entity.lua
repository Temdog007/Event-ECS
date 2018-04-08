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
local ClassName = "Entity"

function string.starts(str, start)
  return string.sub(str, 1, string.len(start)) == start
end

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

  local id = 1

  function entity:getName()
    return entostring(self)
  end

  entity.system = system
  local enabled = true

  function entity:isEnabled() return enabled end
  function entity:setEnabled(pEnabled)
    assert(type(pEnabled) == "boolean", "Must set enabled to a boolean value")
    if enabled ~= pEnabled then
      enabled = pEnabled
      self.entity.system:dispatchEvent("eventEnabledChanged", self, pEnabled)
    end
  end

  local components = {}
  local eventTablesTable = {}

  function entity:addComponent(compName, args)
    local compClass = assert(self.system:getComponent(compName), "Component not found in system")
    local component = assert(compClass(self, args), "Class instance couldn't be created")

    local thisID = id
    component.getID = function() return thisID end
    id = id + 1

    -- Insert component into list of components
    table.insert(components, component:getBase())

    -- Grab all of the components functions and put them into the event tables
    for k,v in pairs(compClass) do
      k = string.lower(k)
      if string.starts(k, "event") and type(v) == "function" then
        if not eventTablesTable[k] then
          eventTablesTable[k] = {}
        end
        table.insert(eventTablesTable[k], {component = component, func = v})
      end
    end

    self.system:dispatchEvent("eventAddedComponent",  {component = component, entity = self})
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
    entity.system:dispatchEvent("eventRemovingComponent", {component = component, entity = entity})
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
    component = component:getBase()
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
    event = string.lower(event)
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
    local value = self.system:removeEntity(self)
    self.system = nil
    self.addComponent = nil
    return value
  end

  function entity:getEventList()
    local tab = {}
    for k in pairs(eventTablesTable) do
      local str = string.gsub(k, "event", "")
      table.insert(tab, str)
    end
    return table.concat(tab, ",")
  end

  function entity:serialize()
    local events = self:getEventList()
    local tab = {}
    if string.len(events) > 0 then
      table.insert(tab, self:getName()..","..self:getEventList())
    else
      table.insert(tab, self:getName())
    end

    for k,v in pairs(components) do
      table.insert(tab, v:serialize())
    end
    return table.concat(tab, "\n")
  end
end)
return Entity
