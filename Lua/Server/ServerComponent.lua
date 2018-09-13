-- Copyright (c) 2018 Temdog007
--
-- Permission is hereby granted, free of charge, to any person obtaining a copy
-- of this software and associated documentation files (the "Software"), to deal
-- in the Software without restriction, including without limitation the rights
-- to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
-- copies of the Software, and to permit persons to whom the Software is
-- furnished to do so, subject to the following conditions:
--
-- The above copyright notice and this permission notice shall be included in all
-- copies or substantial portions of the Software.
--
-- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
-- IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
-- FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
-- AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
-- LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
-- OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
-- SOFTWARE.

local parser = require("messageParser")
local Component = require('component')
local class = require('classlib')

local Systems = require("systemList")

local socket = require("socket")

local serverComponent = class('serverComponent', Component)

local consolePrint = print

local eos = string.char(3)

function serverComponent:__init(entity)
  self:setDefault("name", classname(self))

  entity:setDefaultsAndValues({
    host = "*",
    port = 32485,
    current = 0,
    rate = 1,
    timeout = 0
  })
end

function serverComponent:close()
  local data = self:getData()
  if data.client then data.client:close() end
  if data.server then data.server:close() end
end

function serverComponent:connect()
  self:close()

  local data = self:getData()
  data.client = nil
  data.messages = table.newqueue()

  data.server = assert(socket.bind(data.host, data.port))
  data.server:settimeout(data.timeout)

  local i, p = data.server:getsockname()
  assert(i,p)

  print = function(...)
    consolePrint(...)
    self:enqueueMessages(...)
  end

  print(string.format("Starting server {%s:%u}", data.host, data.port))
end

function serverComponent:eventAddedComponent(args)
  if not args and args.component ~= self then return end

  self:connect()
end

local function serialize(system)
  return system:serialize()
end

function serverComponent:eventUpdateSocket(args)
  args = args or {}

  local data = self:getData()
  data.host = args.host or data.host
  data.port = args.port or data.port
  data.timeout = args.timeout or data.timeout

  self:connect()
end

function serverComponent:eventUpdate(args)
  local data = self:getData()
  if not data.client then
    local err
    data.client, err = data.server:accept()
    if data.client then
      data.client:settimeout(data.timeout)
      print("Found client")
    else
      if err ~= "timeout" then
        print(err)
      end
    end
  end

  if data.client then
    while table.queuesize(data.messages) > 0 do
      local message = table.popleft(data.messages)
      local status, err = pcall(data.client.send, data.client, message)
      if not status then
        print(err)
        data.client = nil
        return
      end
    end

    local l,e = data.client:receive()
    if l then
      parser(l)
    else
      if e ~= "timeout" then
        print(e)
      end
    end

    if not args or not args.dt then return end

    local data = self:getData()
    data.current = data.current + args.dt
    if data.current > data.rate then
      self:enqueueMessages(Systems.forEachSystem(serialize))
      data.current = 0
    end
  end
end

function serverComponent:enqueueMessages(...)
  local data = self:getData()
  for _, m in pairs({...}) do
    local message = tostring(m)
    if message:ends(eos) then
      table.pushright(data.messages, message)
    else
      table.pushright(data.messages, message..eos)
    end
  end
end

function serverComponent:eventQuit(args)
  self:close()
end

lowerEventName(serverComponent)

return serverComponent
