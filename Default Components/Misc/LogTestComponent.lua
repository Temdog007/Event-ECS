local Component = require("component")
local class = require("classlib")

local LogTest = class("logTestComponent", Component)

local defaultColor = {r = 1,g = 1,b = 1,a = 1}
function LogTest:__init(entity)
  self.Component:__init(entity, self)
  self.text = "This is a test string"
  self.showFps = false
  self.x = 0
  self.y = 0
  self.rotation = 0
  self.scaleX = 1
  self.scaleY = 1
  self.space = 10
  self.drawOrder = 0
end

function LogTest:eventAddedComponent(args)
  if args.component == self then
    print("print Test Component Added")
	end
end

function LogTest:eventDraw(args)
  if not args or args.drawOrder ~= self.drawOrder then return end
  
	local color = self:getComponent("colorComponent")
	if not color then color = defaultColor end
	love.graphics.setColor(color)
	if self.showFps then
		love.graphics.print(love.timer.getFPS(), self.x, self.y, self.rotation, self.scaleX, self.scaleY)
		love.graphics.print(self.text, self.x, self.y + self.space, self.rotation, self.scaleX, self.scaleY)
	else
		love.graphics.print(self.text, self.x, self.y, self.rotation, self.scaleX, self.scaleY)
	end
end

function LogTest:eventBroadcast(args)
	args.number = love.math.random()
	print("EventBroadcast handled. Going to dispatch a test event")
	print("Set args to "..tostring(args.number))
	BroadcastEvent("eventTest", args)
end

function LogTest:eventTest(args)
	print("Received "..tostring(args.number).." in event test handler")
end

function LogTest:eventRemovingComponent(args)
  self.Component:eventRemovingComponent(args)
  if args.component == self then
    print("print Test Removing component")
  end
end

return  LogTest
