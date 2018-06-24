local System = require(DEBUG_MODE and "debugSystem" or "system")
local systems = require("systemList")

local system1, system2, system3 = System("Default"), System("Test 1"), System("Test2")

local en = system1:createEntity()
en:addComponent("FinalizerComponent")
en:addComponent("LogTestComponent")
en:addComponent("DebugComponent")
en:addComponent("ColorComponent")

systems.addSystems(system1, system2, system3)