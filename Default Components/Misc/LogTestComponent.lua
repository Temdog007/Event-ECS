local Component = require("component")
local class = require("classlib")

local LogTest = class("logTestComponent", Component)

function LogTest:__user_init()
  self:setDefault("name", classname(self))
  
  local entity = self:getEntity()

  entity.name = classname(self)
  entity.text = "This is a test string"
  entity.showFps = false
  entity.x = 0
  entity.y = 0
  entity.rotation = 0
  entity.scaleX = 1
  entity.scaleY = 1
  entity.space = 10
  entity.drawOrder = 0

  local values = entity.values or {}
  values.showFps = true
  values.x = true
  values.y = true
  values.rotation = true
  values.scaleX = true
  values.scaleY = true
  values.space = true
  values.drawOrder = true
  entity.values = values
end

function LogTest:eventAddedComponent(args)
  if args.component == self then
    print("print Test Component Added")
	end
end

function LogTest:eventDraw(args)
  if not args or args.drawOrder ~= self.drawOrder then return end

	local color = self:get("color")
	if color then
	   love.graphics.setColor(color)
   end

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
  if args.component == self then
    print("print Test Removing component")
  end
end

lowerEventName(LogTest)

return  LogTest
