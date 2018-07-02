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

local testComponent = class("testComponent", Component)

function testComponent:__init(entity)
  self.Component:__init(entity, self)
  self.addedComponentCalled = 0
  self.removingComponentCalled = 0
  self.text = ""
  self.x = 0
  self.y = 0
  self.space = 10
  self.added = false
end

function testComponent:eventAddedComponent(args)
  if args.component == self then
    self.added = true
  end
  self.addedComponentCalled = self.addedComponentCalled + 1
end

function testComponent:eventDraw(args)
  local color = assert(self:getComponent("colorComponent"), "No color")
  love.graphics.setColor(color)
  love.graphics.print(love.timer.getFPS(), self.x, self.y)
  love.graphics.print(love.timer.getDelta(), self.x, self.y + self.space)
  love.graphics.print(self.text, self.x, self.y + self.space * 2)
  love.graphics.rectangle("fill", self.x, self.y + self.space * 3, 100, 100)
end

function testComponent:eventKeyPressed(args)
  if args[1] == "escape" then
    love.event.quit("Exiting because 'escape' was pressed")
  elseif args[1] == "f9" then
    error("Error was thrown on purpose because of pressing F9")
  end
  self.text = args[1]
end

function testComponent:eventError(args)
  error("Event error was called")
end

function testComponent:addedComponent(args)
  error("This shouldn't have been called")
end

function testComponent:addedComponentEvent(args)
  error("This shouldn't have been called")
end

function testComponent:removingComponent(args)
  error("This shouldn't have been called")
end

function testComponent:eventRemovingComponent(args)
  self.Component:eventRemovingComponent(args)
  if args.component == self then
    self.added = false
  end
  self.removingComponentCalled = self.removingComponentCalled + 1
end

return testComponent
