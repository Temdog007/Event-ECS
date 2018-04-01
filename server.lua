local Class = require("classFactory")
local socket = require("socket")
local Log = require("logging")

return Class(function(server, system, args)
  assert(system)

  function server:__tostring()
    return "System Server"
  end

  args = args or {}
  local host = args.host or "*"
  local port = args.port or 9999
  local timeout = args.timeout or 1
  local log = args.log

  local s = assert(socket.bind(host, port))
  s:settimeout(timeout)

  local runInstance = coroutine.create(function()
    local done = false
    local lastConnection = os.clock()
    while not done do
      log:info("Acquiring new client")
      local client = s:accept()
      if client then

        if log then
          log:info("Accepted connection. Waiting for command")
        end

        local message, status
        while not status do

          message, status = client:receive()
          if message then
            message = string.lower(message)
            if log then
              log:info("Received '%s' from client", message)
            end
          end

          if message == "getdata" then
            if log then
              log:info("Sending system data")
            end
            assert(client:send(system:encode()))
          elseif message == "close" then
            if log then
              log:info("Closing connection")
            end
            done = true
            break
          elseif message then
            assert(client:send(message))
          end
        end

        if log then
          if status then
            log:info(status)
          end
          log:info("Done with client")
        end

      else
        log:warn("Client accept timed out")
      end
      coroutine.yield(lastConnection)
    end
  end)

  server.run = function()
    return coroutine.resume(runInstance)
  end
end)
