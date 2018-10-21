require.config({
  baseUrl : "../..",
  paths :
  {
    game : "Tests/game",
    drawableComponent : 'Default Components/Interfaces/drawableComponent',
    Box2D_v2_3_1 : "Default Components/Physics/Box2D_v2_3_1",
    box2d : "Default Components/Physics/box2d",
    box2dBody : "Default Components/Physics/box2dBody",
    box2dWorld : "Default Components/Physics/box2dWorld",
    WorldComponent : "Default Components/Physics/Components/WorldComponent",
    BodyComponent : "Default Components/Physics/Components/BodyComponent",
    BodyDrawerComponent : "Default Components/Physics/Components/BodyDrawerComponent"
  }
});

require([
  'game', 'system', 'systemlist', 'box2d',
  'WorldComponent', 'BodyComponent', 'BodyDrawerComponent'
],
function(Game, _, Systems, box2d, WorldComponent, BodyComponent, BodyDrawerComponent)
{
  var system = Systems.addSystem("Gamepad Test");
  var entity = system.createEntity();
  entity.addComponent(WorldComponent);

  for(var i = 0; i < 3; ++i)
  {
    var entity = system.createEntity();
    var arr = entity.addComponents([BodyComponent, BodyDrawerComponent]);

    var body = arr[1].body;
    switch(i)
    {
      case 0:
        var shape = new box2d.b2PolygonShape();
        shape.SetAsBox(400, 50);
        body.CreateFixture(shape, 10);
        body.SetTransform(new box2d.b2Vec2(canvas.width / 2, canvas.height - 50), 0);
        break;
      case 1:
        var shape = new box2d.b2CircleShape();
        shape.set_m_radius(100);
        var f = body.CreateFixture(shape, 10);
        body.SetTransform(new box2d.b2Vec2(canvas.width / 2, canvas.height / 2), 0);
        body.SetType(box2d.b2_dynamicBody);
        f.SetRestitution(0.75);
        break;
      case 2:
        var shape = new box2d.b2EdgeShape();
        shape.Set(new box2d.b2Vec2(200, 200), new box2d.b2Vec2(100, 100));
        body.CreateFixture(shape, 10);
        break;
      // case 3:
      //   console.log(box2d);
      //   var shape = new box2d.b2ChainShape();
      //   var buffer = box2d.allocate(32, 'float', box2d.ALLOC_STACK);
      //   var vertices = [0, 0, 800, 0, 800, 100, 0, 100];
      //   for(var i = 0; i < 8; i += 2)
      //   {
      //     box2d.setValue(buffer + (i * 4), vertices[i], 'float');
      //     box2d.setValue(buffer + ((i+1) * 4), vertices[i+1], 'float');
      //   }
      //   buffer = box2d.wrapPointer(buffer, 4);
      //   shape.CreateLoop(buffer, 4);
      //   body.CreateFixture(shape, 10);
      //   break;
    }
  }
});
