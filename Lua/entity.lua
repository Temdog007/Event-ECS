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

local class = require("classlib")
local component = class("Component")
local ClassName = "Entity"
local entity = class(ClassName)

function string.starts(str, start)
  return string.sub(str, 1, string.len(start)) == start
end

function string.split(str, sep)
  local sep, fields = sep or "|", {}
  local pattern = string.format("([^%s]+)", sep)
  string.gsub(str, pattern, function(c)
    table.insert(fields, c)
  end)
  return fields
end

function string.ends(str, endStr)
  return  string.sub(str, -string.len(endStr)) == endStr
end

local function removeAll()
  return true
end

local function entostring(en)
  return en.name or ClassName
end

local function addComponentsFromTable(s, t, i)
  i = i or 1
  if t[i] ~= nil then
    return s:addComponent(t[i]), addComponentsFromTable(s, t, i + 1)
  end
end

local function removeComponentFunction(entity, index, component)
  entity.system:dispatchEvent("eventRemovingComponent", {component = component, entity = entity})
  entity.components[index] = nil

  for eventName,eventTables in pairs(entity.eventTablesTable) do
    for k, eventTable in pairs(eventTables) do
      if eventTable.component == component then
        eventTables[k] = nil
      end
    end
  end
  entity.system:dispatchEvent("eventRemovedComponent", {component = component, entity = entity})
end

function entity:__init(system)
  assert(system, "Entity must have a system")

  self.system = system

  self.enabled = true
  self.components = {}
  self.eventTablesTable = {}
end

function entity:isEnabled()
  return self.enabled
end

function entity:setEnabled(pEnabled)
  assert(type(pEnabled) == "boolean", "Must set self.enabled to a boolean value")
  if self.enabled ~= pEnabled then
    self.enabled = pEnabled
    self.system:dispatchEvent("eventEnabledChanged", {entity = self, components = self.components, enabled = pEnabled})
  end
end

function entity:addComponents(...)
  return addComponentsFromTable(self, {...})
end

function entity:removeComponent(component)
  if component == nil then error("Cannot remove nil") end

  if type(component) == "number" then
    local id = component
    component = assert(self:findComponent(id), string.format("A component with ID %d not found", id))
  end

  local name = classname(component)
  if self[name] then self[name] = nil end
  local base = component:getBase()
  for k,v in pairs(self.components) do
    if v == base then
      removeComponentFunction(self, k, component)
      return true
    end
  end
  return false
end

function entity:removeComponents(compFunc)
  for k,component in pairs(self.components) do
    if compFunc(component) then
      removeComponentFunction(self, k, component)
    end
  end
end

function entity:componentCount()
  local count = 0
  for _ in pairs(self.components) do
    count = count + 1
  end
  return count
end

function entity:findComponent(pArg)

  local matchFunction
  if type(pArg) == "number" then
    matchFunction = function(en) return en:getID() == pArg end
  elseif type(pArg) == "function" then
    matchFunction = pArg
  else
    error(string.format("Must pass function or number to findComponent. Passed %s", type(pArg)))
  end

  for _, component in pairs(self.components) do
    if matchFunction(component) then
      return component.parent or component
    end
  end

end

function entity:findComponents(matchFunction)
  local tab = {}
  for _, component in pairs(self.components) do
    if matchFunction(component) then
      tab[#tab + 1] = component
    end
  end
  return tab
end

function entity:dispatchEvent(event, args)
  event = string.lower(event)
  local eventTables = self.eventTablesTable[event]
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
  local value = self.system:removeEntity(self)
  self:removeComponents(removeAll)
  self.system = nil
  return value
end

function entity:getEventList()
  local tab = {}
  for k in pairs(self.eventTablesTable) do
    local str = string.gsub(k, "event", "")
    table.insert(tab, str)
  end
  return table.concat(tab, "|")
end

function entity:serialize()
  local events = self:getEventList()
  local tab = {}
  if string.len(events) > 0 then
    table.insert(tab, string.format("Entity|%d|%s|%s|%s", self:getID(), tostring(self:isEnabled()), self:getName(), self:getEventList()))
  else
    table.insert(tab, string.format("Entity|%d|%s|%s", self:getID(), tostring(self:isEnabled()), self:getName()))
  end

  for _,v in pairs(self.components) do
    table.insert(tab, v:serialize())
  end
  return table.concat(tab, "\n")
end

function entity:getName()
  return entostring(self)
end

function entity:addComponent(comp, args)
  local compClass
  if type(comp) == "string" then
    compClass = assert(require(comp), string.format("A component '%s' couldn't be found", tostring(comp)))
  else
    compClass = comp
  end

  local component = assert(compClass(self, args), string.format("Instance of '%s' couldn't be created", tostring(compClass)))

  -- Insert component into list of self.components
  table.insert(self.components, component:getBase())

  -- Grab all of the self.components functions and put them into the event tables
  for k,v in pairs(compClass) do
    k = string.lower(k)
    if string.starts(k, "event") and type(v) == "function" then
      if not self.eventTablesTable[k] then
        self.eventTablesTable[k] = {}
      end
      table.insert(self.eventTablesTable[k], {component = component, func = v})
    end
  end

  self.system:dispatchEvent("eventAddedComponent",  {component = component, entity = self})
  return component
end

return entity
