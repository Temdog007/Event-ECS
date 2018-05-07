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
local class = require("classlib")

local TestComponent = class("TestComponent", Component)
local FinalComponent = class("FinalComponent", Component)

--
-- FinalComponent
--
function FinalComponent:__init(entity)
  self.Component:__init(entity, self)
end

function FinalComponent:eventRemovingEntity(args)
  if self:getEntity() == args.entity then
    error("Cannot remove this entity")
  end
end

function FinalComponent:eventRemovingComponent(args)
  if self == args.component then
    error("Cannot remove this component")
  end
end

--
--TestComponent
--
function TestComponent:__init(entity)
  self.Component:__init(entity, self)
  self.addedComponentCalled = 0
  self.removingComponentCalled = 0
end

function TestComponent:eventAddedComponent(args)
  if args.component == self then
    self.added = true
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
  self.Component:eventRemovingComponent(args)
  if args.component == self then
    self.added = false
  end
  self.removingComponentCalled = self.removingComponentCalled + 1
end

function assertIs(actual, expected)
  assertEquals(type(actual), expected)
end

function assertIsTrue(actual)
  assertEquals(actual, true)
end

function assertNotIsTrue(actual)
  assertError(assertEquals, actual, true)
end

function assertIsFalse(actual)
  assertEquals(actual, false)
end

function assertNotIsFalsee(actual)
  assertError(assertEquals, actual, false)
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

function assertMatch(actual, likewise)
  assertIsTrue(string.match(actual, likewise))
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

  assertNotIsNil(entity.components)
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

  assertIs(system.entities, "table")
  assertIsNil(system.addEntity)
  assertIs(system.createEntity, "function")
  assertIs(system.removeEntity, "function")
  assertIs(system.removeEntities, "function")
  assertIs(system.findEntity, "function")
  assertIs(system.findEntities, "function")
  assertIs(system.dispatchEvent, "function")
  assertIs(system.registerComponent, "function")
  assertIs(system.registerComponents, "function")

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
  local entity = system:createEntity()
  assertError(entity.addComponent, entity, 'TestComponent')

  system:registerComponent(TestComponent)
  local comp1 = entity:addComponent('TestComponent')
  assertEquals(classname(TestComponent), classname(comp1))
  assertEquals(entity:componentCount(), 1)
  assertIsNil(comp1.entity)
  assertIsTrue(comp1:getEntity() == entity)
  assertIsTrue(comp1:getSystem() == system)
  assertIs(comp1:getID(), "number")
  assertEquals(comp1.addedComponentCalled, 1)
  assertIsTrue(entity:removeComponent(comp1))
  assertEquals(comp1.removingComponentCalled, 1)
  assertEquals(entity:componentCount(), 0)
  assertNotIsNil(system:findEntity(function(en) return en == entity end))

  local comp2 = entity:addComponent('TestComponent')
  assertEquals(entity:componentCount(), 1)
  assertIsNil(comp2.entity)
  assertIsTrue(comp2:getEntity() == entity)
  assertIsTrue(comp2:getSystem() == system)
  assertIs(comp2:getID(), "number")
  assertEquals(comp2.addedComponentCalled, 1)
  assertEquals(comp2.removingComponentCalled, 0)
  assertIsTrue(entity:remove())
  assertEquals(comp2.removingComponentCalled, 1)
  assertEquals(entity:componentCount(), 0)
  assertIsNil(system:findEntity(function(en) return en == entity end))
  assertIsNil(comp1.entity)
  assertEquals(comp1.addedComponentCalled, 1)
  assertNotIsTrue(entity:removeComponent(comp1))
  assertEquals(comp1.removingComponentCalled, 1)

  entity = system:createEntity()
  assertIsTrue(entity:isEnabled())
  assertEquals(entity:componentCount(), 0)
  local comp3 = entity:addComponent('TestComponent')
  assertIsNil(comp3.entity)
  assertIsTrue(comp3:getEntity() == entity)
  assertIsTrue(comp3:getSystem() == system)
  assertIs(comp3:getID(), "number")
  assertEquals(comp3.addedComponentCalled, 1)
  assertIsTrue(comp3:remove())
  assertEquals(comp3.removingComponentCalled, 1)
  assertEquals(entity:componentCount(), 0)

  local comp4 = entity:addComponent('TestComponent')
  assertIsNil(comp4.entity)
  assertIsTrue(comp4:getEntity() == entity)
  assertIsTrue(comp4:getSystem() == system)
  assertIs(comp4:getID(), "number")
  assertEquals(comp3.addedComponentCalled, 1)
  assertIsTrue(system:removeEntity(entity:getID()))
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

  assertNotIsNil(entity:findComponent(comp1:getID()))
  assertNotIsNil(entity:findComponent(comp2:getID()))
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

  assertNotIsNil(system:findEntity(entity1:getID()))
  assertNotIsNil(system:findEntity(entity2:getID()))
  assertNotIsNil(system:findEntity(entity3:getID()))
end

function ecsTests:testEntityComponents4()
  local system = System()
  system:registerComponent(FinalComponent)
  local entity = system:createEntity()
  local comp = entity:addComponent("FinalComponent")
  assertEquals(entity:componentCount(), 1)
  assertError(comp.remove, comp)
  assertEquals(entity:componentCount(), 1)
  assertError(entity.remove, comp)
  assertEquals(entity:componentCount(), 1)
end

function ecsTests:testEntityFunctions()
  local system = System()
  local entity = system:createEntity()
  assertError(entity.addComponent, entity)
  assertError(entity.addComponent, entity, 6)
end

function ecsTests:testSystemSerialization()
  local system = System()
  assertEquals(system:serialize(), "Entity Component System|Component")
  system:registerComponent(TestComponent)
  assertEquals(system:serialize(), "Entity Component System|Component|TestComponent")
end

function ecsTests:testAddingRemovingEntities()
  local system = System()
  local entity1 = system:createEntity()
  local entity2 = system:createEntity()

  assertEquals(entity1:componentCount(), 0)
  assertEquals(entity2:componentCount(), 0)

  system:registerComponent(TestComponent)
  entity1:addComponent('TestComponent')

  assertEquals(entity1:componentCount(), 1)
  assertEquals(entity2:componentCount(), 0)
end

-- function ecsTests:testEntitySerialization()
--   local system = System()
--   system:registerComponent(TestComponent)
--   local entity = system:createEntity()
--
--   assertEquals(entity:serialize(), "1|Entity")
--   local comp = entity:addComponent("Component")
--   assertMatch(entity:serialize(), "1|Entity|removingentity|removingcomponent\nComponent|enabled|boolean|true")
--   comp = entity:addComponent("TestComponent")
--   assertMatch(entity:serialize(), "1|Entity|removingcomponent|removingentity|addedcomponent\nComponent|enabled|boolean|true\nTestComponent|enabled|boolean|true|removingComponentCalled|number|0|addedComponentCalled|number|1|added|boolean|true")
--   assertMatch(system:serialize(), "Entity Component System|Component|TestComponent\n1|Entity|removingcomponent|removingentity|addedcomponent\nComponent|enabled|boolean|true\nTestComponent|enabled|boolean|true|removingComponentCalled|number|0|addedComponentCalled|number|1|added|boolean|true")
--
--   local entity2 = system:createEntity()
--   assertEquals(entity2:serialize(), "2|Entity")
-- end

LuaUnit:run('ecsTests')
