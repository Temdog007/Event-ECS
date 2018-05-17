local Component = require("component")
local class = require("classlib")

local LogTest = class("LogTestComponent", Component)

function LogTest:__init(entity)
  self.Component:__init(entity, self)
  self.text = "This string is unused. Only set for testing"
  self.x = 0
  self.y = 0
  self.space = 10
  self.color = entity.ColorComponent
end

function LogTest:eventAddedComponent(args)
  if args.component == self then
    Log("Log Test Component Added")
  elseif args.entity == self:getEntity() and classname(args.component) == "ColorComponent" then
	self.color = args.component
  end
end

function LogTest:eventDraw(args)
love.graphics.setColor(self.color.r, self.color.g, self.color.b, self.color.a)
love.graphics.print(love.timer.getFPS(), self.x, self.y)
love.graphics.print(self.text, self.x, self.y + self.space)
end

function LogTest:eventRemovingComponent(args)
  self.Component:eventRemovingComponent(args)
  if args.component == self then
    Log("Log Test Removing component")
  end
end

return  LogTest
