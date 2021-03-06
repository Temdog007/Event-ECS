define(['DrawableComponent'], function(DrawableComponent)
{
  class SquareDrawerCompnent extends DrawableComponent
  {
    constructor(entity)
    {
      super(entity);
      this.setDefaults({
        x: 10,
        y: 25,
        width : 50,
        height : 50,
        color : "pink",
        fill : true
      });
    }

    eventDraw()
    {
      if(this.get("fill"))
      {
        this.context.fillStyle = this.get("color");
        this.context.fillRect(this.get("x"), this.get("y"), this.get("width"), this.get("height"));
      }
      else
      {
        this.context.strokeStyle = this.get("color");
        this.context.strokeRect(this.get("x"), this.get("y"), this.get("width"), this.get("height"));
      }
    }
  }

  return SquareDrawerCompnent;
})
