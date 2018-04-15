local Component = require("component")
local class = require("classlib")

local LogTest = class("LogTestComponent", Component)

function LogTest:__init(entity)
  self.Component:__init(entity, self)
  self.unused = "This is unused. Only set for testing"
end

function LogTest:eventAddedComponent(args)
  if args.component == self then
    Log("Log Test Component Added")
  end
end

function LogTest:eventDraw(args)
love.graphics.print(love.timer.getFPS())
end

function LogTest:eventRemovingComponent(args)
	self.Component:eventRemovingComponent(args)
  if args.component == self then
    Log("Log Test Removing component")
  end
end

return  LogTest
