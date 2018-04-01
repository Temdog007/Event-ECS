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

local LuaUnit = require("luaunit")
local System = require("system")

function assertIsTrue(actual)
  assertEquals(actual, true)
end

function assertNotIsTrue(actual)
  assertError(assertEquals, actual, true)
end

function assertIsNil(actual)
  assertEquals(actual, nil)
end

function assertNotIsNil(actual)
  assertError(assertEquals, actual, nil)
end

function assertNotEquals(actual, notexpected)
  assertError(assertEquals, actual, notexpected)
end

local function addedComponent(c, args)
  if args.component == c then
    c.added = true
    c.entity = args.entity
  end
  c.addedComponentCalled = c.addedComponentCalled + 1
end

local function removingComponent(c, args)
  if args.component == c then
    c.added = false
    c.entity = nil
  end
  c.removingComponentCalled = c.removingComponentCalled + 1
end

local function createComponent()
  return
  {
    addedComponentCalled = 0,
    removingComponentCalled = 0,
    addedComponent = addedComponent,
    removingComponent = removingComponent
  }
end

ecsTests = {}

function ecsTests:testEntityInitialization()
  assertError(function()
    local entity = require("Entity")()
  end)

  local system = System()
  local entity = system:createEntity()

  assertIsNil(entity.components)
  assertIsNil(entity.id)
  assertIsNil(entity.handlingEvent)

  assertNotIsNil(entity.system)

  assertEquals(type(entity.addComponent), "function")
  assertEquals(type(entity.addComponents), "function")
  assertEquals(type(entity.removeComponent), "function")
  assertEquals(type(entity.removeComponents), "function")
  assertEquals(type(entity.getID), "function")
  assertEquals(type(entity.remove), "function")
  assertEquals(type(entity.dispatchEvent), "function")

  assertEquals(tostring(entity), "Entity")
  entity.name = "Test"
  assertNotEquals(tostring(entity), "Entity")
  assertEquals(tostring(entity), "Entity: Test")
end

function ecsTests:testSystemInitialization()
  local system = System()

  assertIsNil(system.entities)
  assertIsNil(system.addEntity)

  assertEquals(type(system.createEntity), "function")
  assertEquals(type(system.removeEntity), "function")
  assertEquals(type(system.removeEntities), "function")
  assertEquals(type(system.findEntity), "function")
  assertEquals(type(system.findEntities), "function")
  assertEquals(type(system.dispatchEvent), "function")

  assertEquals(tostring(system), "Entity Component System")
  system.name = "Unit Test"
  assertNotEquals(tostring(system), "Entity Component System")
  assertEquals(tostring(system), "Entity Component System: Unit Test")
end

function ecsTests:testSystemFind()
  local system = System()
  assertEquals(system:entityCount(), 0)
  assertIsNil(system:findEntity(function() return true end))

  local entity = system:createEntity()
  assertEquals(system:entityCount(), 1)
  assertNotIsNil(system:findEntity(function() return true end))
  assertIsTrue(system:removeEntity(entity))
  assertIsNil(system:findEntity(function(en) return en == entity end))
end

function ecsTests:testEntityComponents()
  local system = System()
  local entity = system:createEntity()
  local comp1 = createComponent()
  local comp2 = createComponent()

  entity:addComponent(comp1)
  assertEquals(entity:componentCount(), 1)
  assertEquals(comp1.entity, entity)
  assertEquals(comp1.addedComponentCalled, 1)
  assertIsTrue(entity:removeComponent(comp1))
  assertEquals(comp1.removingComponentCalled, 1)
  assertEquals(entity:componentCount(), 0)
  assertNotIsNil(system:findEntity(function(en) return en == entity end))

  entity:addComponent(comp2)
  assertEquals(entity:componentCount(), 1)
  assertEquals(comp2.entity, entity)
  assertEquals(comp2.addedComponentCalled, 1)
  assertIsTrue(entity:remove())
  assertEquals(comp2.removingComponentCalled, 1)
  assertEquals(entity:componentCount(), 0)
  assertIsNil(system:findEntity(function(en) return en == entity end))

  assertIsNil(comp1.entity)
  assertEquals(comp1.addedComponentCalled, 2)
  assertNotIsTrue(entity:removeComponent(comp1))
  assertEquals(comp1.removingComponentCalled, 2)
end

function ecsTests:testEntityComponents2()
  local system = System()
  local entity = system:createEntity()
  local comp1 = createComponent()
  local comp2 = createComponent()

  assertEquals(entity:componentCount(), 0)
  entity:addComponents(comp1, comp2)
  assertEquals(entity:componentCount(), 2)

  assertEquals(comp1.addedComponentCalled, 2)
  assertEquals(comp2.addedComponentCalled, 1)
end

function ecsTests:testEntityFunctions()
  local system = System()
  local entity = system:createEntity()
  assertError(entity.addComponent, entity)
  assertError(entity.addComponent, entity, 6)
end

function ecsTests:testEvents()
  local system = System()
  local entity = system:createEntity()
  local comp1 = createComponent()
  local comp2 = createComponent()
  local comp3 = createComponent()

  local function testEvent(c, args)
    args.handled = args.handled + 1
  end
  comp1.testEvent = testEvent
  comp2.testEvent = testEvent

  entity:addComponents(comp1, comp2, comp3)

  local args = {handled = 0}
  local count = system:dispatchEvent("testEvent", args)
  assertEquals(count, 2)
  assertEquals(args.handled, 2)
end

LuaUnit:run('ecsTests')
