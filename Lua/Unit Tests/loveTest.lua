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

package.preload['conf'] = function()
  return function(t)
    t.window.vsync = 0
  end
end

local LoveSystem = require("loveSystem")
local loveSystem = LoveSystem("Unit Test")

local Component = require("component")
local class = require("classlib")

local TestComponent = require("Unit Tests/testComponent")

loveSystem:registerComponent(TestComponent)
loveSystem.frameRate = 120
loveSystem:run()

local entity = loveSystem:createEntity()
entity:addComponent("TestComponent")

repeat loveSystem:draw() until not loveSystem:run()

repeat until not loveSystem:run()

loveSystem = LoveSystem("Unit Test")
repeat loveSystem:draw() until not loveSystem:run()

print("Corutine ended")
