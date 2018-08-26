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
  return string.sub(str, -string.len(endStr)) == endStr
end

function string.unCamelCase(str)
  local tab = {}
  for c in str:gmatch(".") do
    if #tab == 0 then
      table.insert(tab, string.upper(c))
    elseif c:match("^[A-Z]") then
      table.insert(tab, " ")
      table.insert(tab, string.upper(c))
    else
      table.insert(tab, c)
    end
  end
  return table.concat(tab, "")
end

function lowerEventName(t)
  local temp = {}
  for k,v in pairs(t) do
    if string.starts(k, "event") and type(v) == "function" then
      temp[string.lower(k)] = v
    end
  end
  for k,v in pairs(temp) do
    if not t[k] then
      t[k] = v
    end
  end
end
