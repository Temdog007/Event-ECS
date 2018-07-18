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

function serverComponent:__init()
  self.host = "*"
  self.port = 32485
  self.timeout = 0

  self.current = 0
  self.rate = 1
end

function serverComponent:close()
  if self.client then self.client:close() end
  if self.server then self.server:close() end
end

function serverComponent:connect()
  self:close()

  self.client = nil
  self.messages = {}

  self.server = assert(socket.bind(self.host, self.port))
  self.server:settimeout(self.timeout)

  local i, p = self.server:getsockname()
  assert(i,p)

  print = function(...)
    consolePrint(...)
    self:enqueueMessages(...)
  end

  print(string.format("Starting server {%s:%u}", self.host, self.port))
end

function serverComponent:enqueueMessages(...)
  for _, m in pairs({...}) do
    local message = tostring(m)
    if message:ends(eos) then
      table.insert(self.messages, message)
    else
      table.insert(self.messages, message..eos)
    end
  end
end

function serverComponent:eventAddedComponent(args)
  if not args then return end
  if args.component ~= self then return end

  self:connect()
end

function serverComponent:eventUpdateSocket(args)
  args = args or {}

  self.host = args.host or self.host
  self.port = args.port or self.port
  self.timeout = args.timeout or self.timeout

  self:connect()
end

local function serialize(system)
  return system:serialize()
end

function serverComponent:eventUpdate(args)

  if not self.client then
    local err
    self.client, err = self.server:accept()
    if self.client then
      self.client:settimeout(self.timeout)
      print("Found client")
    else
      if err ~= "timeout" then
        print(err)
      end
    end
  end

  if self.client then
    for i, message in pairs(self.messages) do
      local status, err = pcall(self.client.send, self.client, message)
      if not status then
        print(err)
        self.client = nil
        return
      end
      self.messages[i] = nil
    end

    local l,e = self.client:receive()
    if l then
      parser(l)
    else
      if e ~= "timeout" then
        print(e)
      end
    end

    if not args or not args.dt then return end

    self.current = self.current + args.dt
    if self.current > self.rate then
      self:enqueueMessages(Systems.forEachSystem(serialize))
      self.current = 0
    end
  end
end

function serverComponent:eventQuit(args)
  self:close()
end

return serverComponent
