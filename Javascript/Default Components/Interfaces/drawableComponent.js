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
}
