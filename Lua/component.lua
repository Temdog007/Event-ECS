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

local id = 0
function component:__init(entity, parent)
  assert(entity and string.match(entity:getName(), "Entity"), "Component must have an entity")
  self.entity = entity
  self.enabled = true
  self.parent = parent
  self.id = id
  id = id + 1
end

function component:getID()
  return self.id
end

function component:isEnabled()
  return self.enabled
end

function component:getBase()
  return self
end

function component:getEntity()
  return self.entity
end

function component:getSystem()
  return self:getEntity().system
end

function component:setEnabled(enabled)
  assert(type(enabled) == "boolean", "Must set enabled to a boolean value")
  if self.enabled ~= enabled then
    self.enabled = enabled
    self.entity.system:dispatchEvent("eventEnabledChanged", self, enabled)
  end
end

function component:eventRemovingComponent(args)
  if args.component == self then
    self.entity = nil
    self.enabled = false
  end
end

function component:eventRemovingEntity(args)
  if args.entity == self.entity then
    self:remove()
  end
end

function component:remove()
  return self.entity:removeComponent(self.parent or self)
end

local function contains(tab, value)
  for k,v in pairs(tab) do
    if v == value then
      return true
    end
  end
  return false
end

local function addValues(tab, t, addTables)
  for k,v in pairs(tab) do
    k = tostring(k)
    if not string.starts(k, "__") then
      local typ = type(v)
      if typ == "string" or typ == "number" or typ == "boolean" then
        local val = tostring(v)
        table.insert(t, k)
        table.insert(t, typ)
        table.insert(t, val)
      elseif addTables and typ == "table" then
        if classname(v) then
          print(string.format("%s will not be added", k))
        else
          print(string.format("%s will be added", k))
          addValues(v, t, args)
        end
      end
    end
  end
end

function component:serialize()
  local t =
  {
    classname(self.parent or self),
  }
  if self.parent then
    addValues(self.parent, t, true)
  end
  addValues(self, t)
  return table.concat(t, "|")
end

return component
