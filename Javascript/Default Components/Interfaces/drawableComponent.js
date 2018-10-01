define(['component'], function(Component)
{
  class DrawableComponent extends Component
  {
    constructor(entity)
    {
      super(entity);

      this.setDefaults({drawOrder : 0})
    }

    canDraw(args, drawOrder)
    {
      return args != null && args.drawOrder == this.get("drawOrder");
    }

    eventDraw(args)
    {
      if(!this.canDraw(args)){return;}

      this.doDraw(args);
    }

    doDraw(){}
  }

  return DrawableComponent;
});
