require('eventecs')
require('eventecsserver')

local Server = require("serverComponent")
local systems = require("systemList")

local System = require(DEBUG_MODE and "debugSystem" or "system")

local system = systems.addSystem(System("Server System"))
local en = system:createEntity()
en:set('name', "System Entity")
en:addComponent(Server)
en:addComponent("finalizerComponent")
