class RectangleComponent extends DrawableComponent
{
  doDraw(args)
  {
    context.fillStyle = "blue";
    context.fillRect(0,0,100,100);
  }
}

var system = Systems.addSystem(new System("Test"));
system.registerEntity("Test1", [RectangleComponent, EventTesterComponent]);
system.registerEntity("Test2", [LogTestComponent]);
system.registerEntity("Test3", [FpsGraphComponent]);
system.registerEntity("Test4", [FpsDisplayerComponent]);
system.registerEntity("Test5", [ImageDrawerComponent]);
system.registerEntity("Test6", [CoroutineTesterComponent]);

// var e1 = system.createEntity("Test2");
// e1.set("drawOrder", -1);
// e1.set("width", 200);
// e1.set('x', 0);
// e1.set('y', 0);

var e2 = system.createEntity("Test5");
e2.set("drawable", document.getElementById("testimage"));

var e3 = system.createEntity("Test2");
e3.set("drawOrder", -1);
e3.set("width", 200);
e3.set("y", 400);
e3.set('color', 'blue');

var e = system.createEntity("Test3");
e.set("drawOrder", 0);
e.set("lineColor", "orange");

addDrawOrder(-1);
