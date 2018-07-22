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
local Entity = require("entity")
local EcsObject = require("ecsObject")
local DebugSystem = require("debugSystem")
local class = require("classlib")
require("Unit Tests/assertions")

package.preload["testComponent"] = function() return require("Unit Tests/testComponent") end
package.preload["testComponentAlt"] = function() return require("Unit Tests/testComponentAlt") end
local testComponent = require("Unit Tests/testComponent")
local testComponentAlt = require("Unit Tests/testComponentAlt")

ecsTests = {}

function ecsTests:testParser()
  local system = Systems.addSystem(System("TestSystem"))
  local sysID = system:getID()
  assertError(parser, "AddEntity|1")

  parser(string.format("AddEntity|%d", sysID))
  assertEquals(system:entityCount(), 1)
  assertError(parser, "AddEntity|NoSystem")

  local en = system:findEntity(function() return true end)
  local enID = en:getID()
  parser(string.format('AddComponent|%d|%d|testComponent', sysID, enID))
  assertEquals(en:componentCount(), 1)
  parser("BroadcastEvent|eventerror")

  assertError(parser, "Execute|error('Test error')")

  assertError(parser, "DispatchEvent|%d|eventerror")
  assertError(parser, "DispatchEventEntity|%d|1|eventerror")

  parser(string.format('SetEntityValue|%d|%d|test|test', sysID, enID))
  assertEquals(en:get("test"), "test")
  parser(string.format('SetEntityValue|%d|%d|test|True', sysID, enID))
  assertIsTrue(en:get("test"))
  parser(string.format("SetEntityValue|%d|%d|test|False", sysID, enID))
  assertIsFalse(en:get("test"))
  parser(string.format("SetEntityValue|%d|%d|enabled|FALSE", sysID, enID))
  assertIsFalse(en:isEnabled())
  parser(string.format("DispatchEvent|%d|eventerror", sysID))

  local comp = en:findComponent(function() return true end)
  parser(string.format("RemoveComponent|%d|%d|%d", sysID, enID, comp:getID()))
  assertEquals(en:componentCount(), 0)

  assertError(parser, string.format("RemoveComponent|%d|%d|f32wrfsad", sysID, enID))

  parser(string.format("RemoveEntity|%d|%d", sysID, enID))
  assertEquals(system:entityCount(), 0)
end

function ecsTests:testStringSplit()
  local str = "AddComponent|1|2|3"
  assertEquals(str:split("|"), {"AddComponent", "1", "2", "3"})
  assertIsTrue(str:starts("AddComponent"))
  assertIsTrue(str:ends("2|3"))
end

function ecsTests:testComponents()
  assertEquals(classname(testComponent), "testComponent")
  assertIsTrue(is_a(testComponent, EcsObject))
end

function ecsTests:testEntityInitialization()
  assertError(entity)

  local system = System()
  local entity = system:createEntity()

  assertIsNil(entity.components)
  assertIsNil(entity.id)
  assertIsNil(entity.handlingEvent)
  assertIsNil(entity.system)

  assertIs(entity:get("components"), "table")
  assertIs(entity:get("id"), "number")
  assertIs(entity:get("enabled"), "boolean")

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

  assertIsNil(system.entities)
  assertIs(system:get("entities"), "table")
  assertIsNil(system.addEntity)
  assertIs(system.createEntity, "function")
  assertIs(system.removeEntity, "function")
  assertIs(system.removeEntities, "function")
  assertIs(system.findEntity, "function")
  assertIs(system.findEntities, "function")
  assertIs(system.dispatchEvent, "function")

  assertEquals(system:getName(), "System")
  system.name = "Unit Test"
  assertEquals(system:getName(), "System")
  system:set("name", "Unit Test")
  assertEquals(system:getName(), "Unit Test")
end

function ecsTests:testComponentInitialization()
  local system = System()
  local entity = system:createEntity()
  local comp = entity:addComponent(testComponent)

  assertEquals(entity:get("addedComponentCalled"), 1)
  assertEquals(entity:get("x"), 0)
  assertEquals(entity:get("y"), 0)
  assertEquals(entity:get("removingComponentCalled"), 0)
  assertEquals(entity:get("space"), 10)
  assertEquals(entity:get("added"), false)
end

function ecsTests:testDebugSystem()
  local system = System()
  local en = system:createEntity()
  en:addComponent(testComponent)
  assertError(system.dispatchEvent, "eventError")

  system = DebugSystem()
  local en = system:createEntity()
  en:addComponent(testComponent)
  assertIs(system:dispatchEvent("eventError"), "string")
end

function ecsTests:testRegisterEntity()
  local system = System()
  system:registerEntity("test", testComponent)

  local entity = system:createEntity("test")
  assertIs(entity.testComponent, "table")
  assertEquals(entity:get("addedComponentCalled"), 1)
  assertEquals(entity:get("removingComponentCalled"), 0)

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
  assertIs(comp1:getID(), "number")
  assertEquals(entity:get("addedComponentCalled"), 1)
  assertIsTrue(entity:removeComponent(comp1))
  assertEquals(entity:get("removingComponentCalled"), 1)
  assertEquals(entity:componentCount(), 0)
  assertIsTrue(entity == system:findEntity(function(en) return en == entity end))

  local comp2 = entity:addComponent(testComponentAlt)
  assertEquals(entity:componentCount(), 1)
  assertIsNil(comp2.entity)
  assertIs(comp2:getID(), "number")
  assertEquals(entity:get("addedComponentCalled"), 1)
  assertEquals(entity:get("removingComponentCalled"), 1)
  assertError(entity.removeComponent)
  assertIsTrue(entity:remove())
  assertEquals(entity:get("removingComponentCalled"), 1)
  assertEquals(entity:componentCount(), 0)
  assertIsNil(system:findEntity(function(en) return en == entity end))
  assertIsNil(comp1.entity)
  assertError(entity.addComponent, entity, testComponent)
  assertEquals(entity:get("addedComponentCalled"), 1)
  assertNotIsTrue(entity:removeComponent(comp1))
  assertEquals(entity:get("removingComponentCalled"), 1)

  entity = system:createEntity()
  assertIsTrue(entity:isEnabled())
  assertEquals(entity:componentCount(), 0)
  local comp3 = entity:addComponent(testComponent)
  assertIsNil(comp3.entity)
  assertIs(comp3:getID(), "number")
  assertEquals(entity:get("addedComponentCalled"), 1)
  assertIsTrue(comp3:remove())
  assertIsFalse(comp3:remove())
  assertEquals(entity:get("removingComponentCalled"), 1)
  assertEquals(entity:componentCount(), 0)

  local comp4 = entity:addComponent(testComponent)
  assertIsNil(comp4.entity)
  assertIsTrue(comp4:get("entity") == entity)
  assertIs(comp4:getID(), "number")
  assertIsTrue(system:removeEntity(entity:getID()))
end

function ecsTests:testEntityComponents2()
  local system = System()
  local entity = system:createEntity()

  assertEquals(entity:componentCount(), 0)
  local comp1, comp2 = entity:addComponents(testComponent, testComponentAlt)
  assertEquals(entity:componentCount(), 2)

  assertEquals(entity:get("addedComponentCalled"), 1)

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

function ecsTests:testComponentSerialization()
  local system = System()
  local entity = system:createEntity()
  local comp = entity:addComponent(testComponentAlt)

  assertEquals(comp:serialize(), 'enabled|boolean|true|name|string|testComponentAlt|id|number|10')
  comp:setEnabled(false)
  assertEquals(comp:serialize(), 'enabled|boolean|false|name|string|testComponentAlt|id|number|10')

  comp:set("test", {"test string", n = 4})
  assertEquals(entity:serialize(), 'entity|enabled|boolean|true|name|string|Entity|id|number|9\n'..
                                  'component|enabled|boolean|false|name|string|testComponentAlt|id|number|10')

  comp:get("values").test = true
  assertEquals(entity:serialize(), 'entity|enabled|boolean|true|test|table|{1,string,test string,n,number,4}|name|string|Entity|id|number|9\n'..
                                  'component|enabled|boolean|false|name|string|testComponentAlt|id|number|10')

  entity:get("values").test = false
  assertEquals(entity:serialize(), 'entity|enabled|boolean|true|name|string|Entity|id|number|9\n'..
    'component|enabled|boolean|false|name|string|testComponentAlt|id|number|10')

  assertEquals(system:serialize(),
    "system|enabled|boolean|true|name|string|System|id|number|8\n"..
    'entity|enabled|boolean|true|name|string|Entity|id|number|9\n'..
    'component|enabled|boolean|false|name|string|testComponentAlt|id|number|10')
end

LuaUnit:run('ecsTests')
