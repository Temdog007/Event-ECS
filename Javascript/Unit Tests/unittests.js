class TestComponent extends Component
{
  constructor(entity)
  {
    super(entity);
    this.name = "TestComponent";
    this.setDefault("addedComponentCalls", 0);
    this.setDefault("updateCalls", 0);
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
var systemList = new SystemList();
console.assertEquals(typeof systemList, "object");
console.assertIs(systemList, SystemList);
console.assertIsNot(systemList, EcsObject);
var tempSystem = systemList.addSystem(system);
console.assertEquals(system, tempSystem);

systemList.pushEvent("eventUpdate");
console.assertEquals(entity.get("updateCalls"), 0);
systemList.flushEvents();
console.assertEquals(entity.get("updateCalls"), 1);
console.log("%cSystem List Test Complete", "color:green");

console.log("%cRunning Component Register Test", "color:green");
system.registerEntity("test", TestComponent2);
system.registerEntity("test2", [TestComponent, TestComponent2]);

var entity2 = system.createEntity('test');
console.assertEquals(entity2.componentCount, 1);
console.assertEquals(entity2.get("addedComponentCalls"), 1);

var entity3 = system.createEntity('test2');
console.assertEquals(entity3.componentCount, 2);
console.assertEquals(entity3.get("addedComponentCalls"), 2);

console.assertEquals(system.entityCount, 3);
console.log("%cComponent Register Test Complete", "color:green");
