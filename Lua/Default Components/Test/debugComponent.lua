local Component = require("component")
local class = require("classlib")

local debugComponent = class("debugComponent", Component)

function debugComponent:__user_init(en)
	self:setDefault("name", classname(self))
	self:set("entity", en)
end

function debugComponent:eventToggleVsync()
	local width, height, flags = love.window.getMode()
	flags.vsync = not flags.vsync
	love.window.updateMode(width, height, flags)
end

lowerEventName(debugComponent)

return debugComponent
