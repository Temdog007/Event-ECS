define(['component', 'game'], function(Component, Game)
{
  class DrawableComponent extends Component
  {
    constructor(entity)
    {
      super(entity);

      this.setDefaults({drawOrder : 0})
    }

    set drawOrder(value)
    {
      this.set("drawOrder", value);
      Game.addLayer(value);
    }

    get drawOrder()
    {
      return this.get("drawOrder");
    }

    get layer()
    {
      return Game.getLayer(this.drawOrder);
    }

    get canvas()
    {
      return this.layer.canvas;
    }

    get context()
    {
      return this.layer.context;
    }
  }

  return DrawableComponent;
});
