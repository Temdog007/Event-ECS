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
local entity = class("entity", require("ecsObject"))
local ComponentBase = require("component")
require("stringExtensions")

local function removeAll()
  return true
end

function checkEntity(obj)
  assert(is_a(obj, entity), "Must pass an entity to create a component")
end

function entity:__user_init(system)
  local System = require("system")
  assert(is_a(system, System), "Entity must have a system")

  self:set("name", "Entity")
  self:set("system", system)
  self:set("components", {})
end

function entity:getData(useDefault)

  if not self.dataTable then
    self.dataTable = setmetatable({},
    {
        __index = function(obj, k)
          return self:get(k)
        end,
        __newindex = function(obj, k, v)
          self:set(k,v)
        end
    })
  end

  if not self.defaultDataTable then
    self.defaultDataTable = setmetatable({},
    {
        __index = function(obj, k)
          return self:get(k)
        end,
        __newindex = function(obj, k, v)
          self:setDefault(k,v)
        end
    })
  end

  return useDefault and self.defaultDataTable or self.dataTable
end

local function addComponentsFromTable(s, t, i)
  i = i or 1
  if t[i] ~= nil then
    return s:addComponent(t[i]), addComponentsFromTable(s, t, i + 1)
  end
end

function entity:addComponent(comp)
  local system = self:get("system")
  assert(system, "Entity has no system attached")

  local compClass
  if type(comp) == "string" then
    compClass = assert(require(comp), string.format("A component '%s' couldn't be found", tostring(comp)))
  else
    compClass = comp
  end

  local component = assert(compClass(self), string.format("Instance of '%s' couldn't be created", tostring(compClass)))
  assert(is_a(component, ComponentBase), "Must pass a component or the name of a registered component to addComponent")

  local compName = classname(compClass)
  assert(not self[compName], string.format("Can only add one type of component('%s') to an entity", compName))
  self[compName] = component

  -- Insert component into list of self:get("components")
  table.insert(self:get("components"), component)

  system:dispatchEvent("eventAddedComponent",  {component = component, entity = self})
  return component
end


function entity:addComponents(...)
  return addComponentsFromTable(self, {...})
end

local function removeComponentFunction(entity, index, component)
  local system = entity:get("system")
  assert(system, "Entity has no system attached")

  system:dispatchEvent("eventRemovingComponent", {component = component, entity = entity})
  entity:get("components")[index] = nil
  system:dispatchEvent("eventRemovedComponent", {component = component, entity = entity})
end

function entity:removeComponent(component)
  assert(component, "Cannot remove nil")

  if type(component) == "number" then
    local id = component
    component = self:findComponent(id)
    if not component then return false end
  end

  local name = classname(component)
  if self[name] then self[name] = nil end

  for k,v in pairs(self:get("components")) do
    if v == component then
      removeComponentFunction(self, k, component)
      return true
    end
  end
  return false
end

function entity:removeComponents(compFunc)
  for k,component in pairs(self:get("components")) do
    if compFunc(component) then
      removeComponentFunction(self, k, component)
    end
  end
end

function entity:componentCount()
  local count = 0
  for _ in pairs(self:get("components")) do
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

  for _, component in pairs(self:get("components")) do
    if matchFunction(component) then
      return component.parent or component
    end
  end

end

function entity:findComponents(matchFunction)
  local tab = {}
  for _, component in pairs(self:get("components")) do
    if matchFunction(component) then
      tab[#tab + 1] = component
    end
  end
  return tab
end

local emptyTable = {}
function entity:dispatchEvent(event, args)
  event = string.lower(event)

  if event == "eventvaluechanged" and args.id == self:getID() and table.contains(args.changes or emptyTable, "enabled") then
    self:get("system"):updateEnabledEntities()
  end

  local count = 0
  for _, comp in pairs(self:get("components")) do
    if comp:isEnabled() or (args and args.ignoreEnabled) then
      local func = comp[event]
      if func then
        func(comp, args)
        count = count + 1
      end
    end
  end
  return count
end

function entity:remove()
  local system = self:get("system")
  assert(system, "Entity has no system attached")

  self:removeComponents(removeAll)
  local value = system:removeEntity(self)
  self:set("system", nil)
  return value
end

function entity:serialize()
  local tab = {}

  table.insert(tab, 'entity|'..self.serializable:serialize())

  for _, comp in pairs(self:get("components")) do
    table.insert(tab, 'component|'..comp:serialize())
  end

  return table.concat(tab, "\n")
end

return entity
