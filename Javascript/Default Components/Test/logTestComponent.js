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

    eventUpdate()
    {
      var data = this.data;
      data.x += 10;
      data.x %= canvas.width;
    }

    eventDraw()
    {
      var data = this.data;
      if(data.font != null)
      {
        this.context.font = data.font;
      }
      this.context.fillStyle = data.color;
      this.context.textBaseline = "top";
      this.context.fillText(data.text, data.x, data.y);

      this.context.fillText(Game.frameRate, data.x, data.y + data.space);
    }
  }

  return LogTestComponent;
});
