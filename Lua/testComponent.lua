local Component = require("component")
local class = require("classlib")

local TestComponent = class("TestComponent", Component)

function TestComponent:__init(entity)
  self.Component:__init(entity, self)
  self.addedComponentCalled = 0
  self.removingComponentCalled = 0
end

function TestComponent:eventAddedComponent(args)
  if args.component == self then
    self.added = true
  end
  self.addedComponentCalled = self.addedComponentCalled + 1
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
