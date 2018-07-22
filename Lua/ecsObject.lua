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
local ecsObject = class("ecsObject", require("serializable"))

local id = 1
function ecsObject:__user_init()
  self:setDefault("name", "ECS Object")
  self:setDefault("enabled", true)
  self:setDefault("id", id)
  id = id + 1

  local values = self:get("values") or {}
  values.name = true
  values.enabled = true
  values.id = true
  self:set("values", values)
end

function ecsObject:getID()
  return self:get("id")
end

function ecsObject:isEnabled()
  return self:get("enabled")
end

function ecsObject:setEnabled(pEnabled)
  assert(type(pEnabled) == "boolean", "Must call setEnabled with a boolean")
  if pEnabled ~= self:get("enabled") then
    self:set("enabled", pEnabled)
  end
end

function ecsObject:getName()
  return self:get("name")
end

function ecsObject:setName(name)
  self:set("name", name)
end

return ecsObject
