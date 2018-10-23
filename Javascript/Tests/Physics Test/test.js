require.config({
  baseUrl : "../..",
  paths :
  {
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

  var ppm = 0.01;
  var mpp = 1 / ppm;
  var width = canvas.width * ppm, height = canvas.height * ppm;

  Object.defineProperty(Game, 'ppm', {
    get : function()
    {
      return ppm;
    },
    set : function(value)
    {
      ppm = value;
    }
  });

  Object.defineProperty(Game, 'mpp', {
    get : function()
    {
      return mpp;
    },
    set : function(value)
    {
      mpp = value;
    }
  });

  for(var i = 0; i < 3; ++i)
  {
    var entity = system.createEntity();
    var arr = entity.addComponents([BodyComponent, BodyDrawerComponent]);

    var body = arr[1].body;
    switch(i)
    {
      case 0:
        var shape = new box2d.b2PolygonShape();
        shape.SetAsBox(4, 0.5);
        body.CreateFixture(shape, 0.1);
        body.SetTransform(new box2d.b2Vec2(width / 2, height - 0.5), 0);
        break;
      case 1:
        var shape = new box2d.b2CircleShape();
        shape.set_m_radius(1);
        var f = body.CreateFixture(shape, 0.1);
        body.SetTransform(new box2d.b2Vec2(width / 2, height / 2), 0);
        body.SetType(box2d.b2_dynamicBody);
        f.SetRestitution(0.75);
        break;
      case 2:
        var shape = new box2d.b2EdgeShape();
        shape.Set(new box2d.b2Vec2(2, 2), new box2d.b2Vec2(1, 1));
        body.CreateFixture(shape, 0.1);
        break;
    }
  }
});
