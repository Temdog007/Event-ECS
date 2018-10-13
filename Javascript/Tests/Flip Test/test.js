require.config({
  baseUrl : '../../',
  paths :
  {
    drawableComponent : 'Default Components/Interfaces/drawableComponent',
    game : 'Tests/game'
  }
})

require(
[
  'system', 'drawableComponent', 'systemlist', 'game'
],
function(_, DrawableComponent, Systems, Game)
{
  var img = new Image();
  img.src = "ufo.png";

  class TestComponent extends DrawableComponent
  {
    constructor(entity)
    {
      super(entity);

      this.setDefaults({
        doScale : false,
        x : 0,
        y : 0,
        width : 100,
        height : 100
      });
    }

    doDraw()
    {
      Game.context.save();

      var data = this.data;
      if(this.get("doScale"))
      {
        Game.context.translate(data.x + data.width / 2, data.y + data.height / 2);
        Game.context.scale(-1,1);
        Game.context.drawImage(img, 0, 0, img.width, img.height, -data.width / 2, -data.height / 2, data.width, data.height);
      }
      else
      {
        Game.context.drawImage(img, 0, 0, img.width, img.height, data.x, data.y, data.width, data.height);
      }

      Game.context.restore();
    }
  }

  var system = Systems.addSystem("test");
  var entity = system.createEntity();
  entity.addComponent(TestComponent);

  entity = system.createEntity();
  var comp = entity.addComponent(TestComponent);
  comp.set("x", 100);
  comp.set("doScale", true);
});
