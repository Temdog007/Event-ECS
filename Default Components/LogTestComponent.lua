local Component = require("component")
local class = require("classlib")

local LogTest = class("LogTestComponent", Component)

local defaultColor = {r = 1,g = 1,b = 1,a = 1}
function LogTest:__init(entity)
  self.Component:__init(entity, self)
  self.text = "This string is unused. Only set for testing"
  self.x = 0
  self.y = 0
  self.space = 10
end

function LogTest:eventAddedComponent(args)
  if args.component == self then
    Log("Log Test Component Added")
	end
end

function LogTest:eventDraw(args)
local color = self:getEntity().ColorComponent
if not color then color = defaultColor end
love.graphics.setColor(color.r, color.g, color.b, color.a)
love.graphics.print(love.timer.getFPS(), self.x, self.y)
love.graphics.print(self.text, self.x, self.y + self.space)
end

function LogTest:eventBroadcast(args)
args.number = love.math.random()
if Log then
Log("EventBroadcast handled. Going to dispatch a test event")
Log("Set args to "..tostring(args.number))
end
BroadcastEvent("eventTest", args)
end

function LogTest:eventTest(args)
	if Log then
		Log("Received "..tostring(args.number).." in event test handler")
	end
end

function LogTest:eventRemovingComponent(args)
  self.Component:eventRemovingComponent(args)
  if args.component == self then
    Log("Log Test Removing component")
  end
end

return  LogTest
