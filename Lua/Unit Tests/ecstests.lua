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

local parser = require("Server/messageParser")
local Systems = require("systemList")
local LuaUnit = require("luaunit")
local System = require("system")
local DebugSystem = require("debugSystem")
local Component = require("component")
local class = require("classlib")

package.preload["testComponent"] = function() return require("Unit Tests/testComponent") end
local testComponent = require("Unit Tests/testComponent")
local testComponentAlt = require("Unit Tests/testComponentAlt")

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

function assertNotIsFalse(actual)
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

function ecsTests:testParser()
  local system = Systems.addSystem(System("TestSystem"))
  assertError(parser, "AddEntity|TestSystem3")

  parser("AddEntity|TestSystem")
  assertEquals(system:entityCount(), 1)
  assertError(parser, "AddEntity|NoSystem")

  parser("AddComponent|TestSystem|1|testComponent")
  parser("BroadcastEvent|eventerror")

  assertError(parser, "Execute|error('Test error')")

  assertError(parser, "DispatchEvent|TestSystem|eventerror")
  assertError(parser, "DispatchEventEntity|TestSystem|1|eventerror")

  local en = system:findEntity(1)
  local comp = en:findComponent(function() return true end)
  parser(string.format("SetComponentValue|TestSystem|1|%d|test|test", comp:getID()))
  assertEquals(comp.test, "test")
  parser(string.format("SetComponentValue|TestSystem|1|%d|test|True", comp:getID()))
  assertIsTrue(comp.test)
  parser(string.format("SetComponentValue|TestSystem|1|%d|test|False", comp:getID()))
  assertIsFalse(comp.test)
  parser(string.format("SetComponentValue|TestSystem|1|%d|enabled|FALSE", comp:getID()))
  parser("DispatchEvent|TestSystem|eventerror")
  assertIsFalse(comp:isEnabled())
  parser(string.format("RemoveComponent|TestSystem|1|%d", comp:getID()))

  assertError(parser, "RemoveComponent|TestSystem|1|f32wrfsad")

  parser("SetEntityValue|TestSystem|1|test|entityTest")
  assertEquals(en.test, "entityTest")

  parser("RemoveEntity|TestSystem|1")
  assertEquals(system:entityCount(), 0)

  parser("SetSystemValue|TestSystem|test|systemTest")
  assertEquals(system.test, "systemTest")
end

function ecsTests:testStringSplit()
  local str = "AddComponent|1|2|3"
  assertEquals(str:split("|"), {"AddComponent", "1", "2", "3"})
  assertIsTrue(str:starts("AddComponent"))
  assertIsTrue(str:ends("2|3"))
end

function ecsTests:testComponents()
  assertEquals(classname(testComponent), "testComponent")
  assertIsTrue(is_a(testComponent, Component))
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
  assertEquals(type(entity.isEnabled), "function")
  assertEquals(type(entity.setEnabled), "function")
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

  assertEquals(system:getName(), "Entity Component System")
  system.name = "Unit Test"
  assertNotEquals(system:getName(), "Entity Component System")
  assertEquals(system:getName(), "Unit Test")
end

function ecsTests:testComponentInitialization()
  local system = System()
  local entity = system:createEntity()
  local comp = entity:addComponent(testComponent)

  assertIs(comp.isEnabled, "function")
  assertIs(comp.setEnabled, "function")
  assertIs(comp.getID, "function")
  assertIs(comp.getEntity, "function")
  assertIs(comp.getSystem, "function")
end

function ecsTests:testDebugSystem()
  local system = DebugSystem()

  local en = system:createEntity()
  en:addComponent(testComponent)
  assertIs(system:dispatchEvent("eventError"), "string")
end

function ecsTests:testRegisterEntity()
  local system = System()
  system:registerEntity("test", testComponent)

  local entity = system:createEntity("test")
  assertIs(entity.testComponent, "table")
  assertEquals(entity.testComponent.addedComponentCalled, 1)
  assertEquals(entity.testComponent.removingComponentCalled, 0)

  assertError(system.createEntity, system, "badTest")
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

function ecsTests:testEntityComponents1()
  local system = System()
  local entity = system:createEntity()

  local comp1 = entity:addComponent(testComponent)
  assertError(entity.addComponent, entity, testComponent)
  assertEquals(classname(testComponent), classname(comp1))
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

  local comp2 = entity:addComponent(testComponentAlt)
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
  local comp3 = entity:addComponent(testComponent)
  assertIsNil(comp3.entity)
  assertIsTrue(comp3:getEntity() == entity)
  assertIsTrue(comp3:getSystem() == system)
  assertIs(comp3:getID(), "number")
  assertEquals(comp3.addedComponentCalled, 1)
  assertIsTrue(comp3:remove())
  assertIsFalse(comp3:remove())
  assertEquals(comp3.removingComponentCalled, 1)
  assertEquals(entity:componentCount(), 0)

  local comp4 = entity:addComponent(testComponent)
  assertIsNil(comp4.entity)
  assertIsTrue(comp4:getEntity() == entity)
  assertIsTrue(comp4:getSystem() == system)
  assertIs(comp4:getID(), "number")
  assertEquals(comp3.addedComponentCalled, 1)
  assertIsTrue(system:removeEntity(entity:getID()))
end

function ecsTests:testEntityComponents2()
  local system = System()
  local entity = system:createEntity()

  assertEquals(entity:componentCount(), 0)
  local comp1, comp2 = entity:addComponents(testComponent, testComponentAlt)
  assertEquals(entity:componentCount(), 2)

  assertEquals(comp1.addedComponentCalled, 2)
  assertEquals(comp2.addedComponentCalled, 1)

  assertNotIsNil(entity:findComponent(comp1:getID()))
  assertNotIsNil(entity:findComponent(comp2:getID()))
end

function ecsTests:testEntityComponents3()
  local system = System()
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
  local entity = system:createEntity()
  local comp = entity:addComponent("finalizerComponent")
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

function ecsTests:testAddingRemovingEntities()
  local system = System()
  local entity1 = system:createEntity()
  local entity2 = system:createEntity()

  assertEquals(entity1:componentCount(), 0)
  assertEquals(entity2:componentCount(), 0)

  entity1:addComponent(testComponent)

  assertEquals(entity1:componentCount(), 1)
  assertEquals(entity2:componentCount(), 0)
end

function ecsTests:testSystemSerialization()
  local system = System()
  assertEquals(system:serialize(), "System|Entity Component System|true")
end

function ecsTests:testEntitySerialization()
  local system = System()
  local entity = system:createEntity()

  assertEquals(entity:serialize(), "Entity|1|true|Entity")
  entity:setEnabled(false)
  assertEquals(entity:serialize(), "Entity|1|false|Entity")
  entity.testKey = "testValue"
  assertEquals(entity:serialize(), "Entity|1|false|Entity")

  assertEquals(system:serialize(), "System|Entity Component System|true\nEntity|1|false|Entity")
end

function ecsTests:testComponentSerialization()
  local system = System()
  local entity = system:createEntity()
  local comp = entity:addComponent(testComponentAlt)

  assertEquals(comp:serialize(), 'Component|testComponentAlt|enabled|boolean|true|id|number|4|removingComponentCalled|number|0|text|string|'..
                    '|rate|number|1|y|number|0|added|boolean|true|current|number|0|space|number|10|addedComponentCalled|number|1|x|number|0')
  comp:setEnabled(false)
  assertEquals(comp:serialize(), 'Component|testComponentAlt|enabled|boolean|false|id|number|4|removingComponentCalled|number|0|text|string|'..
                    '|rate|number|1|y|number|0|added|boolean|true|current|number|0|space|number|10|addedComponentCalled|number|1|x|number|0')

  assertEquals(entity:serialize(),
    "Entity|1|true|Entity|removingentity|addedcomponent|update|removingcomponent\n"..
    'Component|testComponentAlt|enabled|boolean|false|id|number|4|removingComponentCalled|number|0|text|string|'..
    '|rate|number|1|y|number|0|added|boolean|true|current|number|0|space|number|10|addedComponentCalled|number|1|x|number|0')

  assertEquals(system:serialize(),
    "System|Entity Component System|true\n"..
    "Entity|1|true|Entity|removingentity|addedcomponent|update|removingcomponent\n"..
    'Component|testComponentAlt|enabled|boolean|false|id|number|4|removingComponentCalled|number|0|text|string|'..
    '|rate|number|1|y|number|0|added|boolean|true|current|number|0|space|number|10|addedComponentCalled|number|1|x|number|0')
end

function ecsTests:testColorComponent()
  local system = System()
  local entity = system:createEntity()
  local comp = entity:addComponent("colorComponent")

  assertIsTrue(entity.colorComponent == comp)
  assertEquals(entity.colorComponent[1], 1)
  assertEquals(entity.colorComponent[2], 1)
  assertEquals(entity.colorComponent[3], 1)
  assertEquals(entity.colorComponent[4], 1)

  entity:dispatchEvent("eventSetColor", {0, 1, 0.5})
  assertIsTrue(entity.colorComponent == comp)
  assertEquals(entity.colorComponent[1], 0)
  assertEquals(entity.colorComponent[2], 1)
  assertEquals(entity.colorComponent[3], 0.5)
  assertEquals(entity.colorComponent[4], 1)

  assertError(comp.set, comp, "red", "black", {})
end

LuaUnit:run('ecsTests')
