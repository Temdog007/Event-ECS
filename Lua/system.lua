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
local system = class("system", require("ecsObject"))

function system:__user_init(name)
  self:set("name", name or "System")
  self:set("entities", {})
  self:set("registeredEntities", {})
  self:set("registeredEntitiesList", {})
  self:set("dispatchEvent", function(eventName, args)
    return self:dispatchEvent(eventName, args)
   end)

   local values = self:get("values") or {}
   values.registeredEntitiesList = true
   self:set("values", values)
end

function system:registerEntity(name, ...)
  assert(not self:get("registeredEntities")[name], string.format("Entity with name '%s' is already registered", tostring(name)))
  self:get("registeredEntities")[name] = {...}

  local reg = self:get("registeredEntitiesList") or {}
  table.insert(reg, name)
  self:set("registeredEntitiesList", reg)
end

function system:createEntity(name)
  assert(not name or self:get("registeredEntities")[name], string.format("Entity with name '%s' is not registered", tostring(name)))

  local entity = Entity(self)
  table.insert(self:get("entities"), entity)
  if name then
    for _, comp in pairs(self:get("registeredEntities")[name]) do
      entity:addComponent(comp)
    end
  end
  return entity
end

function system:removeEntity(entity)
  if type(entity) == "number" then
    for k, en in pairs(self:get("entities")) do
      if en:getID() == entity then
        self:dispatchEvent("eventRemovingEntity", {entity = entity, system = self})
        self:get("entities")[k] = nil
        self:dispatchEvent("eventRemovedEntity", {entity = entity, system = self})
        return true
      end
    end
  else
    checkEntity(entity)

    for k, en in pairs(self:get("entities")) do
      if en == entity then
        self:dispatchEvent("eventRemovingEntity", {entity = entity, system = self})
        self:get("entities")[k] = nil
        self:dispatchEvent("eventRemovedEntity", {entity = entity, system = self})
        return true
      end
    end
  end
end

function system:removeEntities(matchFunction)
  assert(typeof(matchFunction) == "function", "Must enter a function to remove entities")

  local count = 0
  for k, en in pairs(self:get("entities")) do
    if matchFunction(en) then
      self:dispatchEvent("eventRemovingEntity", {entity = en, system = self})
      self:get("entities")[k] = nil
      self:dispatchEvent("eventRemovedEntity", {entity = en, system = self})
      count = count + 1
    end
  end
  return count
end

function system:entityCount()
  local count = 0
  for _ in pairs(self:get("entities")) do
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

  for _, en in pairs(self:get("entities")) do
    if findFunc(en) then
      return en
    end
  end

end

function system:findEntities(findFunc)
  local tab = {}
  for _, en in pairs(self:get("entities")) do
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
  if args then args.callingSystem = self end
  for _, entity in pairs(self:get("entities")) do
    if entity:isEnabled() then
      eventsHandled = eventsHandled + entity:dispatchEvent(event, args)
    end
  end
  return eventsHandled
end

function system:serialize()
  local tab ={}

  table.insert(tab, self.serializable:serialize())

  for _, en in pairs(self:get("entities")) do
    table.insert(tab, en:serialize())
  end
  return "system|"..table.concat(tab, "\n");
end

return system
