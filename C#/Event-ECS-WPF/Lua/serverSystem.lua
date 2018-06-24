for k,v in pairs({...}) do
  if v == "debug" then
    DEBUG_MODE = true
    break
  end
end

local Server = require("ServerComponent")
local System = require(DEBUG_MODE and "system" or "debugSystem")
local systems = require("systemList")

local system = systems.addSystem(System("Server System"))
local en = system:createEntity()
en:addComponent(Server)
