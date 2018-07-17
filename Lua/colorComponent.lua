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

local Component = require("component")
local class = require("classlib")

local colorComponent = class("colorComponent", Component)

function colorComponent:__init(entity)
  self.Component:__init(entity, self)
  self[1] = 1
  self[2] = 1
  self[3] = 1
  self[4] = 1
end

function colorComponent:inverse(invertAlpha)
  local c ={}
  for i = 1, invertAlpha and 4 or 3 do
    c[i] = math.abs(1 - self[i])
  end
  return c
end

function colorComponent:eventSetColor(args)
  if not args then return end
  if args.color then

    assert(type(args.color) == "string", "Color must be a string")
    color = assert(self.getColor(args.color), string.format("Color '%s' was not found", args.color))

    args[1] = color[1]
    args[2] = color[2]
    args[3] = color[3]
    args[4] = nil -- don't change
  else
    if args[1] then
      if type(args[1]) ~= "number" then
        args[1] = nil
      end
    end
    if args[2] then
      if type(args[2]) ~= "number" then
        args[2] = nil
      end
    end
    if args[3] then
      if type(args[3]) ~= "number" then
        args[3] = nil
      end
    end
    if args[4] then
      if type(args[4]) ~= "number" then
        args[4] = nil
      end
    end
  end

  self[1] = args[1] or self[1]
  self[2] = args[2] or self[2]
  self[3] = args[3] or self[3]
  self[4] = args[4] or self[4]
  self:dispatchEvent("eventColorChanged", {color = self, entity = self:getEntity()})
end

function colorComponent:eventSetBackground(args)
  love.graphics.setBackgroundColor(self)
end

return colorComponent
