local Component = require("component")
local class = require("classlib")

local ColorRandomizer = class("colorRandomizerComponent", Component)

function ColorRandomizer:__init()

  local entity = self:getEntity(true)
  entity.current = 0
  entity.interval = 1
  entity.changeAlpha = false

  local values = entity.values or {}
  values.current = true
  values.interval = true
  values.changeAlpha = true
  entity.values = values
end

function ColorRandomizer:eventUpdate(args)
  local entity = self:getEntity()
	local color = entity.color
	if not color then return end

	entity.current = entity.current + args.dt
	if entity.current > entity.interval then
		color[1] = love.math.random()
		color[2] = love.math.random()
		color[3] = love.math.random()
		if entity.changeAlpha then
			color[4] = love.math.random()
		end
		entity.current = 0
	end
end

lowerEventName(ColorRandomizer)

return  ColorRandomizer
