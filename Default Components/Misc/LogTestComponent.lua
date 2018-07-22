local Component = require("drawableComponent")
local class = require("classlib")

local LogTest = class("logTestComponent", Component)

function LogTest:__user_init(en)
  self:setDefault("name", classname(self))
  self:set("entity", en)
  local entity = self:getEntity(true)

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
  values.text = true
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
  if not self:canDraw(args) then return end

  local entity = self:getEntity()

	local color = entity.color
	if color then
	   love.graphics.setColor(color)
   end

	if entity.showFps then
		love.graphics.print(love.timer.getFPS(), entity.x, entity.y, entity.rotation, entity.scaleX, entity.scaleY)
		love.graphics.print(entity.text, entity.x, entity.y + entity.space, entity.rotation, entity.scaleX, entity.scaleY)
	else
		love.graphics.print(entity.text, entity.x, entity.y, entity.rotation, entity.scaleX, entity.scaleY)
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
