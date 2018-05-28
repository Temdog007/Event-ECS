local Component = require("component")
local class = require("classlib")

local ColorRandomizer = class("ColorRandomizerComponent", Component)

function ColorRandomizer:__init(entity)
  self.Component:__init(entity, self)
  self.current = 0
  self.interval = 1
  self.changeAlpha = false
end

function ColorRandomizer:eventUpdate(args)
	local color = self:getEntity().ColorComponent
	if not color then return end
	
	self.current = self.current + args.dt
	if self.current > self.interval then
		color.r = love.math.random()
		color.g = love.math.random()
		color.b = love.math.random()
		if self.changeAlpha then
			color.a = love.math.random()
		end
		self.current = 0
	end
end

return  ColorRandomizer
