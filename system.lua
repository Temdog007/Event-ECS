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

local Class = require("classFactory")
local Entity = require("entity")
local json = require("json")
local ClassName = "Entity Component System"

local function systostring(sys)
  if sys.name then
    return string.format("%s: %s", ClassName, sys.name)
  else
    return ClassName
  end
end

return Class(function(system, serverArgs)
  local entities = {}

  system.__tostring = systostring

  function system:createEntity()
    local entity = Entity(self)
    table.insert(entities, entity)
    self:dispatchEvent("createdEntity", {entity = entity, system = self})
    return entity
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

  function system:encode()
    local rval =
    {
      system =
      {
        name = tostring(self)
      },
    }

    local enData = {}
    for _, en in pairs(entities) do
      enData[en:getID()] = en:getData()
    end

    rval.system.entities = enData
    return json.encode(rval)
  end

  if serverArgs then
    local Server = require("server")
    local server = Server(system, serverArgs)
    system.updateServer = server.run
  end
end)
