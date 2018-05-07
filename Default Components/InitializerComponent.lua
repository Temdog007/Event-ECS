local Component = require("component")
local class = require("classlib")

local InitializerComponent = class("InitializerComponent", Component)

function InitializerComponent:__init(entity)
  self.Component:__init(entity, self)
  
  local en = self:getEntity()
  en:addComponent("FinalizerComponent")
  en:addComponent("LogTestComponent")
  en:addComponent("DebugComponent")
end

function InitializerComponent:eventUpdate()
	self:remove()
end

return InitializerComponent