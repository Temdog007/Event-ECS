local Component = require("component")
local class = require("classlib")

local InitializerComponent = class("InitializerComponent", Component)

function InitializerComponent:__init(entity)
  self.Component:__init(entity, self)
  
  local en = self:getEntity()
	en:addComponent("finalizerComponent")
	en:addComponent("logTestComponent")
	en:addComponent("debugComponent")
	en:addComponent("colorComponent")
end

function InitializerComponent:eventUpdate()
	self:remove()
	if Log then
		Log("Deserialize")
	end
end

return InitializerComponent