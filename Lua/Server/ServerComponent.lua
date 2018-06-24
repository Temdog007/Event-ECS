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

local Component = require('component')
local class = require('classlib')

local socket = require("socket")

local ServerComponent = class('ServerComponent', Component)

function ServerComponent:__init(entity)
  self.Component:__init(entity, self)

  self.host = "localhost"
  self.port = 32485
  self.timeout = 0
  self.rate = 1
  self.current = 0
end

function ServerComponent:connect()
  if self.server then self.server:close() end

  self.client = nil
  self.messages = {}

  self.server = assert(socket.bind(self.host, self.port))
  self.server:settimeout(self.timeout)

  local i, p = self.server:getsockname()
  assert(i,p)

  if not UNIT_TEST then
    print = function(...)
      for _, message in pairs({...}) do
        table.insert(self.messages, message)
      end
    end
  end

  print(string.format("Starting server {%s:%u}", self.host, self.port))
end

function ServerComponent:eventAddedComponent(args)
  if not args then return end
  if args.component ~= self then return end

  self:connect()
end

function ServerComponent:eventUpdateSocket(args)
  args = args or {}

  self.host = args.host or self.host
  self.port = args.port or self.port
  self.timeout = args.timeout or self.timeout

  self:connect()
end

local function serialize(system)
  print("Serialize\n"..system:serialize())
end

local parseMessage

function ServerComponent:eventUpdate(args)

  if not self.client then
    local err
    self.client, err = self.server:accept()
    if self.client then
      print("Found client")
    else
      if err ~= "timeout" then
        print(client, err)
      end
    end
  end

  if self.client then
    for _, message in pairs(self.messages) do
      assert(self.client:send(message))
    end
    self.messages = {}

    local l,e = self.client:receive()
    parseMessage(l)
  end

  if not args or not args.dt then return end

  self.current = self.currnet + args.dt
  if self.current > self.rate then
    forEachSystem(serialize)
  end
end

parseMessage = function()
end

return ServerComponent
