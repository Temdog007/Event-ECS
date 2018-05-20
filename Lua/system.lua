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
local Entity = require("entity")
local Component = require("component")
local FinalizerComponent = require("finalizerComponent")
local ColorComponent = require("colorComponent")
local ClassName = "Entity Component System"
local system = class(ClassName)

function system:__init(name)
  self.entities = {}
  self.id = 1
  self.name = name or ClassName
  self.enabled = true
end

function system:getName()
  return self.name
end

function system:isEnabled()
  return self.enabled
end

function system:setEnabled(value)
  assert(type(value) == "boolean", "Must set enabled to a boolean value")

  self.enabled = value
end

function system:createEntity()
  local entity = Entity(self)
  local thisID = self.id
  entity.getID = function() return thisID end
  self.id = self.id + 1
  self.entities[#self.entities + 1] = entity
  self:dispatchEvent("eventCreatedEntity", {entity = entity, system = self})
  return entity
end

function system:removeEntity(entity)
  if type(entity) == "number" then
    for k, en in pairs(self.entities) do
      if en:getID() == entity then
        self:dispatchEvent("eventRemovingEntity", {entity = entity, system = self})
        self.entities[k] = nil
        self:dispatchEvent("eventRemovedEntity", {entity = entity, system = self})
        return true
      end
    end
  else
    assert(entity and string.match(entity:getName(), "Entity"), "Must enter a entity to remove")
    for k, en in pairs(self.entities) do
      if en == entity then
        self:dispatchEvent("eventRemovingEntity", {entity = entity, system = self})
        self.entities[k] = nil
        self:dispatchEvent("eventRemovedEntity", {entity = entity, system = self})
        return true
      end
    end
  end
end

function system:removeEntities(matchFunction)
  assert(typeof(matchFunction) == "function", "Must enter a function to remove self.entities")

  local count = 0
  for k, en in pairs(self.entities) do
    if matchFunction(en) then
      self:dispatchEvent("eventRemovingEntity", {entity = en, system = self})
      self.entities[k] = nil
      self:dispatchEvent("eventRemovedEntity", {entity = en, system = self})
      count = count + 1
    end
  end
  return count
end

function system:entityCount()
  local count = 0
  for _ in pairs(self.entities) do
    count = count + 1
  end
  return count
end

function system:findEntity(pArg)

  local findFunc
  if type(pArg) == "number" then
    findFunc = function(en) return en:getID() == pArg end
  else
    findFunc = pArg
  end

  for _, en in pairs(self.entities) do
    if findFunc(en) then
      return en
    end
  end

end

function system:findEntities(findFunc)
  local tab = {}
  for _, en in pairs(self.entities) do
    if findFunc(en) then
      tab[#tab + 1] = en
    end
  end
  return tab
end

function system:dispatchEvent(event, args)
  if not self:isEnabled() then
    return 0
  end

  local eventsHandled = 0
  for _, entity in pairs(self.entities) do
    if entity:isEnabled() then
      eventsHandled = eventsHandled + entity:dispatchEvent(event, args)
    end
  end
  return eventsHandled
end

function system:serialize()
  local tab = { self:getName() }
  for _, en in pairs(self.entities) do
    table.insert(tab, en:serialize())
  end
  return table.concat(tab, "\n");
end

return system
