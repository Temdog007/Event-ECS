require.config({
  baseUrl : '../../',
  paths :
  {
    assert : "Tests/Unit Tests/assert"
  }
});
require(['ecsobject', "component", "entity", "system", "assert", 'systemlist'],
function(EcsObject, Component, Entity, System, _, Systems)
{
  class TestComponent extends Component
  {
    constructor(entity)
    {
      super(entity);
      this.setDefault("addedComponentCalls", 0);
      this.setDefault("updateCalls", 0);
    }

    added()
    {
      ++this.data.addedComponentCalls;
    }

    eventUpdate(args)
    {
      ++this.data.updateCalls;
    }
  }

  class TestComponent2 extends TestComponent
  {
  }

  console.log("%cRunning System Test", "color:green");
  var system = new System();
  console.assertEquals(typeof system, "object");
  console.assertIs(system, System);
  console.assertIs(system, EcsObject);
  console.assertIsNot(system, Entity);
  console.assertEquals(system.id, 0);
  console.assertTrue(system.enabled);
  console.assertEquals(system.entityCount, 0);
  console.log("%cSystem Test complete", "color:green");

  console.log("%cRunning Entity Test", "color:green");
  var entity = system.createEntity();
  console.assertEquals(typeof entity, "object");
  console.assertIs(entity, Entity);
  console.assertIs(entity, EcsObject);
  console.assertIsNot(entity, System);
  console.assertEquals(entity.id, 1);
  console.assertTrue(entity.enabled);
  console.assertEquals(entity.componentCount, 0);
  console.assertEquals(system.entityCount, 1);
  console.log("%cEntity Test Complete", "color:green");

  console.log("%cRunning Component Test", "color:green");
  var component = entity.addComponent(TestComponent);
  console.assertEquals(typeof component, "object");
  console.assertIs(component, Component);
  console.assertIs(component, EcsObject);
  console.assertIsNot(component, Entity);
  console.assertNull(component.addedComponentCalls);
  console.assertEquals(component.data.addedComponentCalls, 0);
  console.assertEquals(entity.data.addedComponentCalls, 0);
  console.assertEquals(entity.componentCount, 1);
  console.assertEquals(component.entity.system.flushEvents(), 1);
  console.assertEquals(component.data.addedComponentCalls, 1);
  console.assertEquals(entity.data.addedComponentCalls, 1);
  console.assertError(function() {entity.addComponent(TestComponent)});
  console.log("%cComponent Test Complete", "color:green");

  console.log("%cRunning System List Test", "color:green");
  console.assertEquals(typeof Systems, "object");
  console.assertIsNot(Systems, EcsObject);
  var tempSystem = Systems.addSystem(system);
  console.assertEquals(system, tempSystem);
  console.assertEquals(Systems.systemCount, 1);
  console.assertEquals(system.entityCount, 1);

  Systems.pushEvent("eventUpdate");
  console.assertEquals(entity.get("updateCalls"), 0);
  console.assertEquals(Systems.flushEvents(), 1);
  console.assertEquals(entity.get("updateCalls"), 1);
  console.assertTrue(entity.remove());
  Systems.flushEvents();
  console.assertEquals(system.entityCount, 0);
  console.log("%cSystem List Test Complete", "color:green");

  console.log("%cRunning Component Register Test", "color:green");
  system.registerEntity("test", TestComponent2);
  system.registerEntity("test2", [TestComponent, TestComponent2]);
  console.assertError(function() {system.registerEntity('test')});

  var entity2 = system.createEntity('test');
  console.assertEquals(entity2.get("addedComponentCalls"), 0);
  console.assertEquals(system.flushEvents(), 1);
  console.assertEquals(entity2.componentCount, 1);
  console.assertEquals(entity2.get("addedComponentCalls"), 1);
  console.assertFalse(entity.remove());
  console.assertTrue(entity2.remove());
  Systems.flushEvents();
  console.assertEquals(system.entityCount, 0);

  var entity3 = system.createEntity('test2');
  console.assert(system.flushEvents(), 2);
  console.assertEquals(entity3.componentCount, 2);
  console.assertEquals(system.entityCount, 1);
  console.assertEquals(entity3.get("addedComponentCalls"), 2);
  console.assertTrue(entity3.TestComponent.remove());
  console.assertEquals(entity3.componentCount, 1);
  console.assertTrue(entity3.remove());
  Systems.flushEvents();
  console.assertEquals(system.entityCount, 0);
  console.log("%cComponent Register Test Complete", "color:green");

  console.log("%cRunning System Finding Test", "color:green");
  var system2 = new System("Test System 2");
  console.assertEquals(system2.name, "Test System 2");
  var en = system2.createEntity();
  console.assertEquals(system2.entityCount, 1);
  var list = system2.findEntities(function() {return true;});
  console.assertNotNull(list);
  console.assertTrue(Array.isArray(list));
  console.assertEquals(list.length, 1);
  console.assertEquals(list[0], en);

  en.addComponent(TestComponent);
  console.assertEquals(en.componentCount, 1);
  console.log("%cSystem Finding Test Complete", "color:green");

  console.log("%cRunning System List Finding Test", "color:green");
  Systems.removeAllSystems();
  console.assertEquals(Systems.systemCount, 0);

  var system3 = Systems.addSystem("Test 3");
  console.assertEquals(system3.name, "Test 3");
  console.assertEquals(Systems.systemCount, 1);
  var entity = system3.createEntity();
  entity.addComponent(TestComponent);
  console.assertEquals(Systems.flushEvents(), 1);// Add component event
  for(var i = 0; i < 5; ++i)
  {
    Systems.pushEvent("eventUpdate");
  }
  console.assertEquals(system3._events.length, 5);
  console.assertEquals(entity.get("updateCalls"), 0);
  console.assertEquals(Systems.flushEvents(), 5);
  console.assertEquals(Systems.flushEvents(), 0);
  console.assertEquals(entity.get("updateCalls"), 5);

  system3.enabled = false;
  console.assertEquals(Systems.flushEvents(), 0);
  Systems.pushEvent("eventUpdate");
  console.assertEquals(Systems.flushEvents(), 0);
  console.assertEquals(entity.get("updateCalls"), 5);

  system3.enabled = true;
  console.assertEquals(Systems.flushEvents(), 0);
  console.assertEquals(entity.get("updateCalls"), 5);

  entity.enabled = false;
  console.assertEquals(Systems.flushEvents(), 0);
  Systems.pushEvent("eventUpdate");
  console.assertEquals(Systems.flushEvents(), 0);
  console.assertEquals(entity.get("updateCalls"), 5);
  console.log("%cSystem List Finding Test Complete", "color:green");
});
