define(['drawableComponent'], function(DrawableComponent)
{
  class FpsDisplayerComponent extends DrawableComponent
  {
    constructor(entity)
    {
      super(entity);

      this.setDefaults({
        x : 0,
        y : 0,
        color : "black",
        font : "16px Arial"
      });
    }

    eventDraw(args)
    {
      var data = this.data;
      this.context.fillStyle = data.color;

      this.context.font = data.font;
      this.context.textBaseline = "top";
      this.context.fillText(getFPS(), data.x, data.y);
    }
  }

  return FpsDisplayerComponent;
});
