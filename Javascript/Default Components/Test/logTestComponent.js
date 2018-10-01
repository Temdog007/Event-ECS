define(['drawableComponent', 'game'], function(DrawableComponent, Game)
{
  class LogTestComponent extends DrawableComponent
  {
    constructor(entity)
    {
      super(entity);
      this.setDefaults({
        text : "This is a test string",
        x : 10,
        y : 30,
        font : "30px Arial",
        space : 100,
        color : "black"
      })
    }

    eventAddedComponent(args)
    {
      if(args.component == this)
      {
        console.log("%cprint Game Test Component", "color:black");
      }
    }

    eventUpdate(args)
    {
      var data = this.data;
      data.x += 10;
      data.x %= canvas.width;
    }

    doDraw(args)
    {
      var data = this.data;
      if(data.font != null)
      {
        Game.context.font = data.font;
      }
      Game.context.fillStyle = data.color;
      Game.context.textBaseline = "top";
      Game.context.fillText(data.text, data.x, data.y);

      Game.context.fillText(Game.getFPS(), data.x, data.y + data.space);
    }
  }

  return LogTestComponent;
});
