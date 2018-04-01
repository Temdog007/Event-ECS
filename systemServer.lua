local args = {...}

local System = require("system")
local logger = require("logging")

local log = logging.new(function(self, level, message)
  print(logging.prepareLogMsg(logPattern, os.date(), level, message))
  return true
end)

local system = System({
  host = args[1] or "*",
  port = args[2] or 9999,
  timeout = args[3] or 1,
  log = log
})

local start = os.clock()

while os.clock() - start < 5 do
  local status, result = system.updateServer()
  if not status and result ~= nil then
    log:error("Update Server coroutine failed: '%s'", result)
    break
  else
    result = status
  end
end
