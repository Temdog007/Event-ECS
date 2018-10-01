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

    doDraw(args)
    {
      var data = this.data;
      context.fillStyle = data.color;

      context.font = data.font;
      context.textBaseline = "top";
      context.fillText(getFPS(), data.x, data.y);
    }
  }

  return FpsDisplayerComponent;
});
