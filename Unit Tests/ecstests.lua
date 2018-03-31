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

local function createComponent()
  local comp = {addedComponentCalled = 0, removingComponentCalled = 0}
  comp.addedComponent = function(args)
    if args.component == comp then
      comp.addedComponentCalled = comp.addedComponentCalled + 1
      comp.entity = args.entity
    end
  end
  comp.removingComponent = function(args)
    if args.component == comp then
      comp.removingComponentCalled = comp.removingComponentCalled + 1
    end
  end
  return comp
end

function testEvents()
  local system = System()
  local entity, comp1, comp2
  local addComponentEvent, removingComponentEvent

  lu.assertIsNil(system:findEntity(function() return true end))
  entity = system:createEntity()
  lu.assertNotIsNil(system:findEntity(function() return true end))
  lu.assertIsTrue(system:removeEntity(entity))

  comp1 = createComponent()
  entity = system:createEntity()
  lu.assertIsNil(entity:findComponent(function() return true end))
  entity:addComponent(comp1)
  lu.assertIs(comp1.entity, entity)
  lu.assertIs(comp1.addedComponentCalled, 1)
  lu.assertIsTrue(entity:removeComponent(comp1))
  lu.assertIs(comp1.removingComponentCalled, 1)

  comp2 = createComponent()
  entity = system:createEntity()
  lu.assertIsNil(entity:findComponent(function() return true end))
  entity:addComponent(comp2)
  lu.assertIs(comp2.entity, entity)
  lu.assertIs(comp2.addedComponentCalled, 1)
  lu.assertIsTrue(entity:remove())
  lu.assertIs(comp2.removingComponentCalled, 1)

  lu.assertIs(comp1.addedComponentCalled, 1)
  lu.assertNotIsTrue(entity:removeComponent(comp1))
  lu.assertIs(comp1.removingComponentCalled, 1)
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
