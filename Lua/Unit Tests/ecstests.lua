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
local Component = require("component")
require("classlib")

local TestComponent = class("TestComponent", Component)

function TestComponent:__init(entity)
  self.Component:__init(entity)
  self.addedComponentCalled = 0
  self.removingComponentCalled = 0
end

function TestComponent:eventAddedComponent(args)
  if args.component == self then
    self.added = true
    self.entity = args.entity
  end
  self.addedComponentCalled = self.addedComponentCalled + 1
end

function TestComponent:addedComponent(args)
  error("This shouldn't have been called")
end

function TestComponent:addedComponentEvent(args)
  error("This shouldn't have been called")
end

function TestComponent:removingComponent(args)
  error("This shouldn't have been called")
end

function TestComponent:eventRemovingComponent(args)
  if args.component == self then
    self.added = false
    self.entity = nil
  end
  self.removingComponentCalled = self.removingComponentCalled + 1
end

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

ecsTests = {}

function ecsTests:testComponents()
  assertEquals(classname(TestComponent), "TestComponent")
  assertIsTrue(is_a(TestComponent, Component))
end

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

  assertEquals(type(entity.getID), "function")
  assertEquals(type(entity.addComponent), "function")
  assertEquals(type(entity.addComponents), "function")
  assertEquals(type(entity.removeComponent), "function")
  assertEquals(type(entity.removeComponents), "function")
  assertEquals(type(entity.getID), "function")
  assertEquals(type(entity.remove), "function")
  assertEquals(type(entity.dispatchEvent), "function")
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
  assertEquals(type(system.registerComponent), "function")
  assertEquals(type(system.registerComponents), "function")

  assertEquals(system:getName(), "Entity Component System")
  system.name = "Unit Test"
  assertNotEquals(system:getName(), "Entity Component System")
  assertEquals(system:getName(), "Entity Component System: Unit Test")
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

function ecsTests:testSystemRegister()
  local system = System()
  assertNotIsNil(system:getComponent("Component"))
end

function ecsTests:testEntityComponents1()
  local system = System()
  system:registerComponent(TestComponent)
  local entity = system:createEntity()

  local comp1 = entity:addComponent('TestComponent')
  assertEquals(classname(TestComponent), classname(comp1))
  assertEquals(entity:componentCount(), 1)
  assertEquals(comp1.entity, entity)
  assertEquals(comp1.addedComponentCalled, 1)
  assertIsTrue(entity:removeComponent(comp1))
  assertEquals(comp1.removingComponentCalled, 1)
  assertEquals(entity:componentCount(), 0)
  assertNotIsNil(system:findEntity(function(en) return en == entity end))

  local comp2 = entity:addComponent('TestComponent')
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
  system:registerComponent(TestComponent)
  local entity = system:createEntity()

  assertEquals(entity:componentCount(), 0)
  local comp1, comp2 = entity:addComponents("TestComponent", "TestComponent")
  assertEquals(entity:componentCount(), 2)

  assertEquals(comp1.addedComponentCalled, 2)
  assertEquals(comp2.addedComponentCalled, 1)
end

function ecsTests:testEntityComponents3()
  local system = System()
  system:registerComponent(TestComponent)
  local entity1 = system:createEntity()
  local entity2 = system:createEntity()
  local entity3 = system:createEntity()

  assertNotEquals(entity1:getID(), entity2:getID())
  assertNotEquals(entity1:getID(), entity3:getID())
  assertNotEquals(entity2:getID(), entity3:getID())
end

function ecsTests:testEntityFunctions()
  local system = System()
  local entity = system:createEntity()
  assertError(entity.addComponent, entity)
  assertError(entity.addComponent, entity, 6)
end

function ecsTests:testEncoding()
  local system = System()
  system:registerComponent(TestComponent)
  local entity = system:createEntity()
  local comp1, comp2, comp3 = entity:addComponents("TestComponent", "TestComponent", "TestComponent")

  print(system:encode())
end

LuaUnit:run('ecsTests')