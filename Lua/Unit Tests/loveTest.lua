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

for k,v in pairs(arg) do
  print(k,v)
end



package.preload['conf'] = function()
  return function(t)
  end
end

local LoveSystem = require("loveSystem")
local loveSystem = LoveSystem("Unit Test")

print(love.filesystem.getExecutablePath())

local Component = require("component")
local class = require("classlib")

local DrawComponent = class("DrawComponent", Component)

function DrawComponent:__init(entity)
  self.Component:__init(entity, self)
  self.text = ""
end

function DrawComponent:eventDraw(args)
  love.graphics.print(love.timer.getFPS())
  love.graphics.print(love.timer.getDelta(), 0, 10)
  love.graphics.print(self.text, 0, 20)
end

function DrawComponent:eventKeyPressed(args)
  -- local tab = {}
  -- for k,v in pairs(args) do
  --   table.insert(tab, tostring(k))
  --   table.insert(tab, tostring(v))
  -- end
  -- self.text = table.concat(tab, ",")
  if args[1] == "escape" then
    love.event.quit()
  end
  self.text = args[1]
end

loveSystem:registerComponent(DrawComponent)
local entity = loveSystem:createEntity()
entity:addComponent("DrawComponent")

local result
repeat result = loveSystem:run() until result

print(result)
print("Corutine ended")
