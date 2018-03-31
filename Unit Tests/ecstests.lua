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

local lu = require("Unit Tests/luaunit")
local System = require("system")

function testEntityInitialization()
  lu.assertError(function()
    local entity = require("Entity")()
  end)

  local system = System()
  local entity = system:createEntity()

  lu.assertIsNil(entity.components)
  lu.assertIsNil(entity.id)
  lu.assertIsNil(entity.handlingEvent)

  lu.assertNotIsNil(entity.system)

  lu.assertIs(type(entity.addComponent), "function")
  lu.assertIs(type(entity.addComponents), "function")
  lu.assertIs(type(entity.removeComponent), "function")
  lu.assertIs(type(entity.removeComponents), "function")
  lu.assertIs(type(entity.getID), "function")
  lu.assertIs(type(entity.remove), "function")
  lu.assertIs(type(entity.dispatchEvent), "function")

  lu.assertIs(tostring(entity), "Entity")
end

function testSystemInitialization()
  local system = System()

  lu.assertIsNil(system.entities)
  lu.assertIsNil(system.addEntity)

  lu.assertIs(type(system.createEntity), "function")
  lu.assertIs(type(system.removeEntity), "function")
  lu.assertIs(type(system.removeEntities), "function")
  lu.assertIs(type(system.findEntity), "function")
  lu.assertIs(type(system.findEntities), "function")
  lu.assertIs(type(system.dispatchEvent), "function")

  lu.assertIs(tostring(system), "Entity Component System")
end

function testSystemFind()
  local system = System()
  lu.assertIs(system:entityCount(), 0)
  lu.assertIsNil(system:findEntity(function() return true end))

  local entity = system:createEntity()
  lu.assertIs(system:entityCount(), 1)
  lu.assertNotIsNil(system:findEntity(function() return true end))
  lu.assertIsTrue(system:removeEntity(entity))
  lu.assertIsNil(system:findEntity(function(en) return en == entity end))
end

local function createComponent()
  local comp = {addedComponentCalled = 0, removingComponentCalled = 0}
  comp.addedComponent = function(args)
    if args.component == comp then
      comp.added = true
      comp.entity = args.entity
    end
    comp.addedComponentCalled = comp.addedComponentCalled + 1
  end
  comp.removingComponent = function(args)
    if args.component == comp then
      comp.added = false
      comp.entity = nil
    end
    comp.removingComponentCalled = comp.removingComponentCalled + 1
  end
  return comp
end

function testEntityComponents()
  local system = System()
  local entity = system:createEntity()
  local comp1 = createComponent()
  local comp2 = createComponent()

  entity:addComponent(comp1)
  lu.assertIs(entity:componentCount(), 1)
  lu.assertIs(comp1.entity, entity)
  lu.assertIs(comp1.addedComponentCalled, 1)
  lu.assertIsTrue(entity:removeComponent(comp1))
  lu.assertIs(comp1.removingComponentCalled, 1)
  lu.assertIs(entity:componentCount(), 0)
  lu.assertNotIsNil(system:findEntity(function(en) return en == entity end))

  entity:addComponent(comp2)
  lu.assertIs(entity:componentCount(), 1)
  lu.assertIs(comp2.entity, entity)
  lu.assertIs(comp2.addedComponentCalled, 1)
  lu.assertIsTrue(entity:remove())
  lu.assertIs(comp2.removingComponentCalled, 1)
  lu.assertIs(entity:componentCount(), 0)
  lu.assertIsNil(system:findEntity(function(en) return en == entity end))

  lu.assertIsNil(comp1.entity)
  lu.assertIs(comp1.addedComponentCalled, 2)
  lu.assertNotIsTrue(entity:removeComponent(comp1))
  lu.assertIs(comp1.removingComponentCalled, 2)
end

function testEntityComponents2()
  local system = System()
  local entity = system:createEntity()
  local comp1 = createComponent()
  local comp2 = createComponent()

  lu.assertIs(entity:componentCount(), 0)
  entity:addComponents(comp1, comp2)
  lu.assertIs(entity:componentCount(), 2)

  lu.assertIs(comp1.addedComponentCalled, 2)
  lu.assertIs(comp2.addedComponentCalled, 1)
end

function testEntityFunctions()
  local system = System()
  local entity = system:createEntity()
  lu.assertError(entity.addComponent, entity)
  lu.assertError(entity.addComponent, entity, 6)
end

local runner = lu.LuaUnit.new()
runner:setOutputType("text")
os.exit(runner:runSuite())
