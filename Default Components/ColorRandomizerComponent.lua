local Component = require("component")
local class = require("classlib")

local ColorRandomizer = class("ColorRandomizerComponent", Component)

function ColorRandomizer:__init(entity)
  self.Component:__init(entity, self)
  self.color = entity.ColorComponent
  self.current = 0
  self.interval = 1
  self.changeAlpha = false
end

function ColorRandomizer:eventAddedComponent(args)
   if args.entity == self:getEntity() and classname(args.component) == "ColorComponent" then
	self.color = args.component
  end
end

function ColorRandomizer:eventUpdate(args)
	self.current = self.current + args.dt
	if self.current > self.interval then
		self.color.r = love.math.random()
		self.color.g = love.math.random()
		self.color.b = love.math.random()
		if self.changeAlpha then
			self.color.a = love.math.random()
		end
		self.current = 0
	end
end

function ColorRandomizer:eventRemovingComponent(args)
  self.Component:eventRemovingComponent(args)
  if args.component == self then
    Log("Log Test Removing component")
  end
end

return  ColorRandomizer
