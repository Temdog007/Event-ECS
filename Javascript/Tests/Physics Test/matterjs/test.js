require.config({
  baseUrl : "../../..",
  paths :
  {
    drawableComponent : 'Default Components/Interfaces/drawableComponent',
    matter : "Default Components/Physics/matter_min",
    WorldComponent : "Default Components/Physics/Matter JS Components/WorldComponent",
    BodyComponent : "Default Components/Physics/Matter JS Components/BodyComponent",
    BodyDrawerComponent : "Default Components/Physics/Matter JS Components/BodyDrawerComponent"
  }
});

require([
  'game', 'system', 'systemlist', 'matter',
  'WorldComponent', 'BodyComponent', 'BodyDrawerComponent'
],
function(Game, _, Systems, matter, WorldComponent, BodyComponent, BodyDrawerComponent)
{
  window.Matter = matter;

  var system = Systems.addSystem("Gamepad Test");
  var entity = system.createEntity();
  entity.addComponent(WorldComponent);

  var width = canvas.width, height = canvas.height;

  for(var j = 0; j < 3; ++j)
  {
    var entity = system.createEntity();
    var arr = entity.addComponents([BodyComponent, BodyDrawerComponent]);

    var body = arr[1].body;
    switch(j)
    {
      case 0:
        Matter.Body.setStatic(body, true);
        var verts = [
          Matter.Vector.create(-400, 50),
          Matter.Vector.create(400, 50),
          Matter.Vector.create(400, -50),
          Matter.Vector.create(-400, -50)
        ];
        body.restitution = 0.75;
        Matter.Body.setVertices(body, verts);
        Matter.Body.setPosition(body, {x : width / 2, y : height - 0.5});
        break;
      case 1:
        var verts = [];
        var theta = 2 * Math.PI / 26;
        for(var i = 0; i < 26; ++i)
        {
          var angle = i * theta;
          verts.push(Matter.Vector.create(Math.cos(angle) * 100 + 50, Math.sin(angle) * 100 + 50));
        }
        Matter.Body.setVertices(body, verts);
        Matter.Body.setPosition(body, {x : width / 2, y : height / 2});
        break;
      case 2:
        Matter.Body.setStatic(body, true);
        var verts = [
          Matter.Vector.create(-50, 0),
          Matter.Vector.create(50, 0),
          Matter.Vector.create(50, -1),
          Matter.Vector.create(-50, -1)
        ];
        Matter.Body.setVertices(body, verts);
        Matter.Body.setPosition(body, {x : 50, y : 50});
        Matter.Body.rotate(body, Math.PI / 4);
        console.log(verts, body.vertices);
        break;
    }
  }
});
