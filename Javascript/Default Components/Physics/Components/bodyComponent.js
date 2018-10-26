define(['component', 'WorldComponent'], function(Component, WorldComponent)
{
  class BodyComponent extends Component
  {
    constructor(entity)
    {
      super(entity);
      this.set("body", WorldComponent.instance.createBody());
    }

    get body()
    {
      return this.get("body");
    }

    remove()
    {
      WorldComponent.instance.world.remove(this.body);
    }
  }

  return BodyComponent;
});
