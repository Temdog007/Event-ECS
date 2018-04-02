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

  local function sendToClient(client, msg)
    if log then
      log:info("Sending to client '%s'", msg)
    end
    assert(client:send(msg))
  end

  local commands = setmetatable(
  {
    getdata = function(client)
      sendToClient(client, "SYSTEM_DATA"..system:encode())
    end,

    addentity = function(client)
      local entity = system:createEntity()
      if log then
        log:info("Added entity #'%d'", entity:getID())
      end
      sendToClient(client, "ENTITY_DATA"..entity:encode())
    end,

    removeentity = function(client, message)
      local id = string.gsub(message, "removeentity", "")
      id = tonumber(id)
      if log then
        log:info("Trying to remove entity #%d", id)
      end
      local count = system:removeEntities(function(en)
        return en:getID() == id
      end)
      sendToClient(client, "REMOVED_ENTITY"..tostring(count))
    end,

    close = function()
      return true
    end,

    echo = function(client, message)
      log:warn("Unknown message: '%s'. Echoing back.", message)
      assert(client:send(message))
    end
  },
  {
    __index = function(tab, key)
      for k,v in pairs(tab) do
        if string.match(key, k) then
          return v
        end
      end
      return v.echo
    end
  })

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

            if log then
              log:info("Recevied '%s'", message)
            end
            message = string.lower(message)

            local command = commands[message]
            done = command(client, message)
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
