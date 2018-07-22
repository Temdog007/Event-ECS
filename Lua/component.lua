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

local ecsObject = require("ecsObject")
local class = require("classlib")
require("stringExtensions")
local component = class("component", ecsObject)

function component:__user_init(entity)
  self:set("entity", entity)
end

function component:set(key, value)
  if key == "entity" then self.entity = value
  else self.entity:set(key, value) end
end

function component:get(key)
  if key == "entity" then return self.entity
  else return self.entity:get(key) end
end

function component:getEntity(useDefault)

  if not self.entityTable then
    self.entityTable = setmetatable({},
    {
        __index = function(obj, k)
          local en = self:get("entity")
          if en then return en:get(k) end
          return nil
        end,
        __newindex = function(obj, k, v)
          local en = self:get("entity")
          if en then
            en:set(k,v)
          end
        end
    })
  end

  if not self.defaultEntityTable then
    self.defaultEntityTable = setmetatable({},
    {
        __index = function(obj, k)
          local en = self:get("entity")
          if en then return en:get(k) end
          return nil
        end,
        __newindex = function(obj, k, v)
          local en = self:get("entity")
          if en then
            en:setDefault(k,v)
          end
        end
    })
  end

  return useDefault and self.defaultEntityTable or self.entityTable
end

function component:remove()
  return self.entity:removeComponent(self:getID())
end

lowerEventName(component)

return component
