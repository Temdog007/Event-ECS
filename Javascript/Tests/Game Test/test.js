require.config({
  baseUrl : '../../',
  paths :
  {
    imageDrawerComponent : 'Default Components/Drawing/imageDrawerComponent',
    drawableComponent : 'Default Components/Interfaces/drawableComponent',
    graphableComponent : 'Default Components/Interfaces/graphableComponent',
    eventTesterComponent : 'Default Components/Test/eventTesterComponent',
    coroutineTesterComponent : 'Default Components/Test/coroutineTesterComponent',
    fpsDisplayerComponent : 'Default Components/Test/fpsDisplayerComponent',
    logTestComponent : 'Default Components/Test/logTestComponent',
    fpsGraphComponent : 'Default Components/Test/fpsGraphComponent'
  }
})

require(
[
  'drawableComponent', 'system', 'eventTesterComponent', 'fpsDisplayerComponent',
  'logTestComponent', 'fpsGraphComponent', 'coroutineTesterComponent', 'coroutineTesterComponent',
  'imageDrawerComponent', 'game'
],
function(DrawableComponent, System, EventTesterComponent, FpsDisplayerComponent, LogTestComponent,
  FpsGraphComponent, CoroutineTesterComponent, CoroutineTesterComponent, ImageDrawerComponent, Game)
{
  class RectangleComponent extends DrawableComponent
  {
    eventDraw()
    {
      this.context.fillStyle = "blue";
      this.context.fillRect(0,0,100,100);
    }
  }

  var Systems = require("systemlist");

  var system = Systems.addSystem(new System("Test"));
  system.registerEntity("Test1", [RectangleComponent, EventTesterComponent]);
  system.registerEntity("Test2", [LogTestComponent]);
  system.registerEntity("Test3", [FpsGraphComponent]);
  system.registerEntity("Test4", [FpsDisplayerComponent]);
  system.registerEntity("Test5", [ImageDrawerComponent]);
  system.registerEntity("Test6", [CoroutineTesterComponent]);

  var e1 = system.createEntity("Test2");
  e1.set("drawOrder", -1);
  e1.set("width", 200);
  e1.set('x', 0);
  e1.set('y', 0);

  var e2 = system.createEntity("Test5");
  e2.set("drawable", document.getElementById("testimage"));
  e2.set('x', 300);
  e2.set('y', 500)

  var e3 = system.createEntity("Test2");
  e3.set("drawOrder", -1);
  e3.set("width", 200);
  e3.set("y", 400);
  e3.set('color', 'blue');

  var e = system.createEntity("Test3");
  e.set("lineColor", "orange");
  e.set("drawOrder", -2);

  e = system.createEntity("Test4");
  e.set("y", 500);
  e.set("drawOrder", 0);
  e.set("lineColor", "green");

  Game.addLayers([-2, -1]);
});
