local Component = require("component")
local class = require("classlib")

local FinalizerComponent = class("FinalizerComponent", Component)

function FinalizerComponent:__init(entity)
  self.Component:__init(entity, self)
end

function FinalizerComponent:eventRemovingComponent(args)
	if args.component == self then
		error("Cannot remove finalizer component")
	end
end

function FinalizerComponent:eventRemovingEntity(args)
	if args.entity == self.entity then
		error("Cannot remove finalizer component")
	end
end

return FinalizerComponent