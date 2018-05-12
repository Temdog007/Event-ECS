local Component = require("component")
local class = require("classlib")

local TestComponent = class("TestComponent", Component)

function TestComponent:__init(entity)
  self.Component:__init(entity, self)
  self.addedComponentCalled = 0
  self.removingComponentCalled = 0
  self.text = ""
end

function TestComponent:eventAddedComponent(args)
  if args.component == self then
    self.added = true
  end
  self.addedComponentCalled = self.addedComponentCalled + 1
end

function TestComponent:eventDraw(args)
  love.graphics.print(love.timer.getFPS())
  love.graphics.print(love.timer.getDelta(), 0, 10)
  love.graphics.print(self.text, 0, 20)
end

function TestComponent:eventKeyPressed(args)
  if args[1] == "escape" then
    love.event.quit()
  end
  self.text = args[1]
end

function TestComponent:addedComponent(args)
  error("This shouldn't have been called")
end

function TestComponent:addedComponentEvent(args)
  error("This shouldn't have been called")
end

function TestComponent:removingComponent(args)
  error("This shouldn't have been called")
end

function TestComponent:eventRemovingComponent(args)
  self.Component:eventRemovingComponent(args)
  if args.component == self then
    self.added = false
  end
  self.removingComponentCalled = self.removingComponentCalled + 1
end

return TestComponent
