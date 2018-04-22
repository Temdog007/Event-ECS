local Component = require("component")
local class = require("classlib")

local FinializerComponent = class("FinializerComponent", Component)

function FinializerComponent:__init(entity)
  self.Component:__init(entity, self)
end

function FinializerComponent:eventRemovingComponent(args)
	if args.component == self then
		error("Cannot remove finializer component")
	end
end