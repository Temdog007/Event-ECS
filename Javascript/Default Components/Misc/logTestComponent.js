class LogTestComponent extends DrawableComponent
{
  constructor(entity)
  {
    super(entity);
    this.setDefaults({
      text : "This is a test string",
      x : 10,
      y : 50,
      font : "30px Arial",
      space : 100,
      color : "black"
    })
  }

  eventAddedComponent(args)
  {
    if(args.component == this)
    {
      console.log("print Game Test Component");
    }
  }

  eventUpdate(args)
  {
    var data = this.data;
    data.x += 10;
    data.x %= canvas.width;
  }

  eventDraw(args)
  {
    if (!this.canDraw(args)) {return;}

    var data = this.data;
    if(data.font != null)
    {
      context.font = data.font;
    }
    context.fillStyle = data.color;
    context.fillText(data.text, data.x, data.y);

    context.fillText(Systems.getFps(), data.x, data.y + data.space);
  }
}
