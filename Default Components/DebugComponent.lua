local Component = require("component")
local class = require("classlib")

local DebugComponent = class("DebugComponent", Component)

function DebugComponent:__init(entity)
  self.Component:__init(entity, self)
end

function DebugComponent:eventToggleVsync()
	local width, height, flags = love.window.getMode()
	flags.vsync = not flags.vsync
	love.window.updateMode(width, height, flags)
end
  
return DebugComponent