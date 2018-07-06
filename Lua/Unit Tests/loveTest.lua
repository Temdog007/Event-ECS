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

arg[0] = "E:\\Game Development\\GitHub\\Event-ECS\\Lua\\Unit Tests"
for k,v in pairs(arg) do print(k,v) end
local love = require ("love")
local loveRoutine = require("love.boot")
local bootFunction = coroutine.create(loveRoutine)

local DebugSystem = require("system")
local testComponent = require("Unit Tests/testComponent")
local System = require("system")
local Systems = require("systemList")

local system = Systems.addSystem(System("Love Test System"))
local en = system:createEntity()
local testComp = en:addComponent(testComponent)
testComp.x = 500
testComp.y = 500
testComp.text = "Test text"
en:addComponent("colorComponent")
assert(en.colorComponent, "Color not added")
en:dispatchEvent("eventsetcolor", {color = "greenyellow"})


local result, err
repeat
  result, err = coroutine.resume(bootFunction)
  if err then print(err) end
until not result
