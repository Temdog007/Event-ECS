local Component = require("component")
local class = require("classlib")

local debugComponent = class("debugComponent", Component)

function debugComponent:__init(entity)
  self.Component:__init(entity, self)
end

function debugComponent:eventToggleVsync()
	local width, height, flags = love.window.getMode()
	flags.vsync = not flags.vsync
	love.window.updateMode(width, height, flags)
end

return debugComponent
