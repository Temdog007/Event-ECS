require('eventecs')
require('eventecsserver')

local Server = require("serverComponent")
local systems = require("systemList")

for k,v in pairs({...}) do
  if v == "debug" then
    DEBUG_MODE = true
    break
  end
end


local System = require(DEBUG_MODE and "system" or "debugSystem")

local system = systems.addSystems(System("Server System"))
local en = system:createEntity()
en:addComponent(Server)
