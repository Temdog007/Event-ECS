local System = require("system")
local logger = require("logging")

local log = logging.new(function(self, level, message)
  print(logging.prepareLogMsg(logPattern, os.date(), level, message))
  return true
end)

local system = System({
  host = arg[1] or "*",
  port = arg[2] or 9999,
  timeout = arg[3] or 1,
  log = log
})

local start = os.clock()

while os.clock() - start < 5 do
  local status, result = system.updateServer()
  if status and type(result) == "number" then
    start = result
  else
    if result ~= nil then
      log:error("Update Server coroutine failed: '%s'", result)
      break
    end
    start = os.clock()
  end
  log:info("Last message sent: %d", start)
end
