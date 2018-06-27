local Component = require("component")
local class = require("classlib")

local ColorRandomizer = class("colorRandomizerComponent", Component)

function ColorRandomizer:__init(entity)
  self.Component:__init(entity, self)
  self.current = 0
  self.interval = 1
  self.changeAlpha = false
end

function ColorRandomizer:eventUpdate(args)
	local color = self:getComponent("colorComponent")
	if not color then return end

	self.current = self.current + args.dt
	if self.current > self.interval then
		color[1] = love.math.random()
		color[2] = love.math.random()
		color[3] = love.math.random()
		if self.changeAlpha then
			color[4] = love.math.random()
		end
		self.current = 0
	end
end

return  ColorRandomizer
