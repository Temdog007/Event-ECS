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

function table.isEmpty(str)
  for _ in pairs(str) do
    return false
  end
  return true
end

function table.copy(t)
  local rval = setmetatable({}, getmetatable(t))
  for k,v in pairs(t) do
    rval[k] = v
  end
  return rval
end

function table.random(t)
  return t[love.math.random(#t)]
end

function table.contains(t, value)
  for k,v in pairs(t) do
    if k == value or v == value then
      return true
    end
  end
end

function table.newqueue()
  return {first = 0, last = -1}
end

function table.queuesize(t)
  if t.first > t.last then return 0 end
  return t.last - t.first + 1
end

function table.pushleft(t, value)
  local first = t.first - 1
  t.first = first
  t[first] = value
end

function table.pushright(t, value)
  local last = t.last + 1
  t.last = last
  t[last] = value
end

function table.popleft(t)
  local first = t.first
  if first > t.last then error("queue is empty") end

  local value =  t[first]
  t[first] = nil
  t.first = first + 1
  return value
end

function table.popright(t)
  local last = t.last
  if t.first > last then error("queue is empty") end

  local value = t[last]
  t[last] = nil
  t.last = last - 1
  return value
end

function table.queuepairs(t)
  local i = t.first-1
  local n = t.last
  return function()
    i = i + 1
    if i <= n then
      return t[i]
    end
  end
end
