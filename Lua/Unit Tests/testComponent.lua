local Component = require("component")
local class = require("classlib")

local TestComponent = class("TestComponent", Component)

function TestComponent:__init(entity)
  self.Component:__init(entity, self)
  self.addedComponentCalled = 0
  self.removingComponentCalled = 0
  self.text = ""
  self.x = 0
  self.y = 0
  self.space = 10
  self.color = entity.ColorComponent
end

function TestComponent:eventAddedComponent(args)
  if args.component == self then
    self.added = true
  elseif classname(args.component) == "ColorComponent" and args.entity == self:getEntity() then
    self.color = args.component
  end
  self.addedComponentCalled = self.addedComponentCalled + 1
end

function TestComponent:eventDraw(args)
  love.graphics.setColor(self.color.r, self.color.g, self.color.b, self.color.a)
  love.graphics.print(love.timer.getFPS(), self.x, self.y)
  love.graphics.print(love.timer.getDelta(), self.x, self.y + self.space)
  love.graphics.print(self.text, self.x, self.y + self.space * 2)
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

-- function TestComponent:eventDraw(args)
--   error("Test error")
-- end

function TestComponent:eventRemovingComponent(args)
  self.Component:eventRemovingComponent(args)
  if args.component == self then
    self.added = false
  end
  self.removingComponentCalled = self.removingComponentCalled + 1
end

return TestComponent
