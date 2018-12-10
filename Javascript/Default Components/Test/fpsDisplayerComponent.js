define(['drawableComponent', 'game'], function(DrawableComponent, Game)
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

    eventDraw()
    {
      var data = this.data;
      this.context.fillStyle = data.color;

      this.context.font = data.font;
      this.context.textBaseline = "top";
      this.context.fillText(Game.frameRate, data.x, data.y);
    }
  }

  return FpsDisplayerComponent;
});
