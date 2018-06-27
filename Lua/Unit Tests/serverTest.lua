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

local System = require("system")
local system = System("Server Test System")
local serverComponent = require("Server/serverComponent")
local socket = require("socket")

local entity = system:createEntity()
local server = entity:addComponent(serverComponent)
function forEachSystem(serializeFunc)
  if server.client then
    server.client:send("Test message")
  end
end

print("Starting Server test")

local sleepLength = 1 / 1000
local args = {dt = 1 / 60}
local start = os.time()
local current = start
while current - start < 5 do
  system:dispatchEvent("eventupdate", args)
  socket.sleep(args.dt)
  current = os.time()
end

system:dispatchEvent("eventquit")
print("Server test complete")
