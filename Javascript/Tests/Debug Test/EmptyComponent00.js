define(['DrawableComponent'], function(DrawableComponent)
{
  class EmptyComponent00 extends DrawableComponent
  {
    eventDraw()
    {
      this.context.strokeStyle = "magenta";
      this.context.strokeRect(10, 25, 50, 50);
    }
  }

  return EmptyComponent00;
})
