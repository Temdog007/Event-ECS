require.config({
  baseUrl : "../../",
  paths :
  {
    game : "Tests/game"
  }
});

require(['system', 'systemlist', 'component', 'game'], function(System, Systems, Component, Game)
{
  class Rect extends Component
  {
    constructor(entity)
    {
      super(entity);
      this.setDefaults({
        x : 0,
        y : 0,
        r : 0,
        w : 100,
        h : 100,
        rotate : true,
        color : "black",
        center : false
      });
    }

    eventUpdate(args)
    {
      if(this.data.rotate)
      {
        this.data.r += args.dt;
      }
    }

    eventDraw()
    {
      var data = this.data;

      Game.context.save();
      if(data.center)
      {
        Game.context.translate(data.x, data.y);
      }
      else
      {
        Game.context.translate(data.x + data.w / 2, data.y + data.h / 2);
      }
      Game.context.rotate(data.r);
      Game.context.fillStyle = data.color;
      Game.context.fillRect(-data.w / 2, -data.h / 2, data.w, data.h);
      Game.context.restore();
    }
  }

  var system = Systems.addSystem("Test");

  function makeEntity()
  {
    var entity = system.createEntity();
    var comp = entity.addComponent(Rect);
    return entity;
  }

  var cx = Game.canvas.width / 2, cy = Game.canvas.height / 2;

  var entity1 = makeEntity();
  entity1.set("color", "blue");
  entity1.set("x", cx);
  entity1.set("y", cy);

  var entity2 = makeEntity();
  entity2.set("color", "green");
  entity2.set("x", cx + 100);
  entity2.set("y", cy);
  entity2.set("center", true);

  var entity3 = makeEntity();
  entity3.set("rotate", false);
  entity3.set("x", cx);
  entity3.set("y", cy);
  entity3.set("center", true);
});