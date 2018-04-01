local socket = require("socket")

return Class(function(server, system, args)
  assert(system)

  args = args or {}
  local host = args.host or "*"
  local port = args.port or 9999
  local timeout = args.timeout or 1

  local s = assert(socket.bind(host, port))
  s:settimeout(timeout)

  local runInstance = coroutine.create(function()
    while true do
      local client = s:accept()
      if client then
        local message = c:receive()
        if message == "getData" then
          client:send(system:encode())
        elseif message == "close" then
          break
        end
      end
      coroutine.yield()
    end
  end)

  server.run = function(self)
    return pcall(coroutine.resume, runInstance)
  end
end)
