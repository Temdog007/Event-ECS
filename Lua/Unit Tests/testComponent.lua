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

local component = require("component")
local class = require("classlib")

local testComponent = class("testComponent", component)

function testComponent:__user_init(entity)
  checkEntity(entity)

  self:setDefault("base", self)

  self:setDefault("name", "testComponent")

  entity:setDefault("addedComponentCalled", 0)
  entity:setDefault("x", 0)
  entity:setDefault("y", 0)
  entity:setDefault("removingComponentCalled", 0)
  entity:setDefault("space", 10)
  entity:setDefault("added", false)
end

function testComponent:eventAddedComponent(args)
  if not args or not args.callingEntity then return end

  if args.component == self then
    local en = args.callingEntity
    local v = en:get("addedComponentCalled")
    en:set("addedComponentCalled", v + 1)
  end
end

function testComponent:eventRemovingComponent(args)
  if not args or not args.callingEntity then return end

  if args.component == self then
    local en = args.callingEntity
    local v = en:get("removingComponentCalled")
    en:set("removingComponentCalled", v + 1)
  end
end

function testComponent:eventDraw(args)
  if not args then return end

  local entity = self:getEntity()
  if args.drawOrder >= 0 then
    local x, y = entity.x + 25 * args.drawOrder, entity.y + 25 * args.drawOrder
    local color = entity.color
    if color then love.graphics.setColor(color) end
    love.graphics.print(love.timer.getFPS(), x, y)
    love.graphics.print(love.timer.getDelta(), x, y + entity.space)
    love.graphics.print(entity.text, x, y + entity.space * 2)
  end
end

local order = 0
function testComponent:eventKeyPressed(args)
  if not args or not args.callingEntity then return end

  if args[1] == "escape" then
    love.event.quit("Exiting because 'escape' was pressed")
  elseif args[1] == "f1" then
    while not addDrawOrder(order) do order = order + 1 end
  elseif args[1] == "f2" then
    removeDrawOrder(order)
    order = order - 1
  elseif args[1] == "f3" then
    addDrawOrder("Bad order")
  elseif args[1] == "f4" then
    removeDrawOrder("Bad Order")
  elseif args[1] == "f9" then
    error("Error was thrown on purpose because of pressing F9")
  end
  args.entity:set("text", args[1])
end

function testComponent:eventError(args)
  error("Event error was called")
end

function testComponent:addedComponent(args)
  error("This shouldn't have been called")
end

function testComponent:addedComponentEvent(args)
  error("This shouldn't have been called")
end

function testComponent:removingComponent(args)
  error("This shouldn't have been called")
end

lowerEventName(testComponent)

return testComponent
