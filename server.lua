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
      local status, result = pcall(function()
        local id = string.gsub(message, "removeentity", "")
        if id then
          id = tonumber(id)
        end
        if id then return id else
          error("Must send id of entity to remove")
        end
      end)

      if not status and result then
        sendToClient(client, result)
      else
        if log then
          log:info("Trying to remove entity #%d", result)
        end
        local count = system:removeEntities(function(en)
          return en:getID() == result
        end)
        sendToClient(client, "REMOVED_ENTITY"..tostring(count))
      end
    end,

    close = function(client)
      sendToClient(client, "CLOSING")
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
      return tab.echo
    end
  })

  local runInstance = coroutine.create(function()
    local done = false
    local lastConnection = os.clock()
    while not done do
      if log then
        log:info("Acquiring new client")
      end

      local client = s:accept()
      if client then

        if log then
          log:info("Accepted connection. Waiting for command")
        end

        local message, status
        while not status and not done do
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

      elseif log then
        log:warn("Client accept timed out")
      end
      coroutine.yield(lastConnection)
    end
  end)

  server.run = function()
    return coroutine.resume(runInstance)
  end
end)
