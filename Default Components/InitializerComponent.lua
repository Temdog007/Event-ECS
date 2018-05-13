local Component = require("component")
local class = require("classlib")

local InitializerComponent = class("InitializerComponent", Component)

function InitializerComponent:__init(entity)
  self.Component:__init(entity, self)
  
  local en = self:getEntity()
  en:addComponent("FinalizerComponent")
  en:addComponent("LogTestComponent")
  en:addComponent("DebugComponent")
  en:addComponent("ColorComponent")
end

function InitializerComponent:eventUpdate()
	self:remove()
	if Log then
		Log("Deserialize")
	end
end

return InitializerComponent