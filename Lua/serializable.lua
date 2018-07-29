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
local serializable = class("serializable")

function serializable:__init()
  self:setDefault("values", {})
end

function serializable:set(name, value)
  assert(name, "Must enter a name in set() function")
  rawset(self, name, value)
end

function serializable:get(name)
  assert(name, "Must enter a name in get() function")
  return rawget(self, name)
end

function serializable:setDefault(key, value)
  if self:get(key) == nil and value ~= nil then
    self:set(key, value)
  end
end

function serializable:setDefaults(args)
  assert(type(args) == "table", "Must pass a table to setDefaults")
  for k,v in pairs(args) do
    self:setDefault(k,v)
  end
end

function serializable:setDefaultsAndValues(args)
  assert(type(args) == "table", "Must pass a table to setDefaultsAndValues")
  local values = self:get("values") or {}
  for k,v in pairs(args) do
    self:setDefault(k,v)
    values[k] = true
  end
  self:set("values", values)
end

local function serializeTable(t, delim)
  delim = delim or ","
  local rval = {}
  for k,v in pairs(t) do
    k = tostring(k)
    local typ = type(v)
    if typ == "string" or typ == "number" or typ == "boolean" then
      table.insert(rval, k) -- key
      table.insert(rval, typ) -- type
      table.insert(rval, tostring(v)) -- value
    elseif classname(v) then
      table.insert(t, k) -- key
      table.insert(t, "string") -- type
      table.insert(t, classname(v)) -- value
    elseif typ == "table" then
      table.insert(rval, k) -- key
      table.insert(rval, "table")
      table.insert(rval, serializeTable(v))
    end
  end
  return "{"..table.concat(rval, delim).."}"
end

function serializable:serialize(t, delim)
  delim = delim or "|"
  t = t or {}
  for k, set in pairs(self:get("values") or {}) do
    if set then
      k = tostring(k)
      local v = self:get(k)
      local typ = type(v)
      if typ == "string" or typ == "number" or typ == "boolean" then
        table.insert(t, k) -- key
        table.insert(t, typ) -- type
        table.insert(t, tostring(v)) -- value
      elseif classname(v) then
        table.insert(t, k) -- key
        table.insert(t, "string") -- type
        table.insert(t, classname(v)) -- value
      elseif typ == "table" then
        table.insert(t, k) -- key
        table.insert(t, "table")
        table.insert(t, serializeTable(v))
      end
    end
  end
  return table.concat(t, delim)
end

return serializable
