local System = require("system")

return function()
	local system1, system2, system3 = System("Default"), System("Test 1"), System("Test2")
	
	local en = system1:createEntity()
	en:addComponent("finalizerComponent")
	en:addComponent("logTestComponent")
	en:addComponent("debugComponent")
	en:addComponent("colorComponent")
	return system1, system2, system3
end