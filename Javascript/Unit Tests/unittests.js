class TestComponent extends Component
{
  constructor(entity)
  {
    super(entity);
    this.name = "TestComponent";
    this.setDefault("addedComponentCalls", 0);
    this.setDefault("updateCalls", 0);
    this.system.set("dispatchEventOnValueChange", true);
    this.set("dispatchEventOnValueChange", true);
    this.setDefault("enabledChanged", 0);
  }

  eventAddedComponent(args)
  {
    if(args.component.id == this.id)
    {
      ++this.data.addedComponentCalls;
    }
  }

  eventUpdate(args)
  {
    ++this.data.updateCalls;
  }

  eventValueChanged(args)
  {
    if(args.changes.enabled)
    {
      if(this.entity.id == args.id)
      {
        if(this.en != this.entity.enabled)
        {
          ++this.data.enabledChanged;
          this.en = this.entity.enabled;
        }
      }
      else if(this.system.id == args.id)
      {
        if(this.sy != this.system.enabled)
        {
          ++this.data.enabledChanged;
          this.sy = this.system.enabled;
        }
      }
    }
  }
}

class TestComponent2 extends TestComponent
{
  constructor(entity)
  {
    super(entity);
    this.name = "TestComponent2";
  }
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
console.assertEquals(component.data.addedComponentCalls, 1);
console.assertEquals(entity.data.addedComponentCalls, 1);
console.assertEquals(entity.componentCount, 1);
console.assertError(function() {entity.addComponent(TestComponent)});
console.log("%cComponent Test Complete", "color:green");

console.log("%cRunning System List Test", "color:green");
var Systems = new SystemList();
console.assertEquals(typeof Systems, "object");
console.assertIs(Systems, SystemList);
console.assertIsNot(Systems, EcsObject);
var tempSystem = Systems.addSystem(system);
console.assertEquals(system, tempSystem);

Systems.pushEvent("eventUpdate");
console.assertEquals(entity.get("updateCalls"), 0);
Systems.flushEvents();
console.assertEquals(entity.get("updateCalls"), 1);
console.assertTrue(entity.remove());
console.assertEquals(system.entityCount, 0);
console.log("%cSystem List Test Complete", "color:green");

console.log("%cRunning Component Register Test", "color:green");
system.registerEntity("test", TestComponent2);
system.registerEntity("test2", [TestComponent, TestComponent2]);
console.assertError(function() {system.registerEntity('test')});

var entity2 = system.createEntity('test');
console.assertEquals(entity2.componentCount, 1);
console.assertEquals(entity2.get("addedComponentCalls"), 1);
console.assertFalse(entity.remove());
console.assertTrue(entity2.remove());
console.assertEquals(system.entityCount, 0);

var entity3 = system.createEntity('test2');
console.assertEquals(entity3.componentCount, 2);
console.assertEquals(system.entityCount, 1);
console.assertEquals(entity3.get("addedComponentCalls"), 2);
console.assertTrue(entity3.TestComponent.remove());
console.assertEquals(entity3.componentCount, 1);
console.assertTrue(entity3.remove());
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
console.assertEquals(Systems.count, 0);

var system3 = Systems.addSystem(new System("Test 3"));
console.assertEquals(system3.name, "Test 3");
console.assertEquals(Systems.count, 1);
var entity = system3.createEntity();
entity.addComponent(TestComponent);
for(var i = 0; i < 5; ++i)
{
  Systems.pushEvent("eventUpdate");
}
Systems.flushEvents();
console.assertEquals(entity.get("updateCalls"), 5);
system3.enabled = false;
Systems.pushEvent("eventUpdate");
Systems.flushEvents();
console.assertEquals(entity.get("enabledChanged"), 1);
console.assertEquals(entity.get("updateCalls"), 5);

system3.enabled = true;
Systems.pushEvent("eventUpdate");
Systems.flushEvents();
console.assertEquals(entity.get("enabledChanged"), 2);
console.assertEquals(entity.get("updateCalls"), 6);

entity.enabled = false;
Systems.pushEvent("eventUpdate");
Systems.flushEvents();
console.assertEquals(entity.get("enabledChanged"), 3);
console.assertEquals(entity.get("updateCalls"), 6);
console.log("%cSystem List Finding Test Complete", "color:green");
