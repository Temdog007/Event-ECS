define(['box2d', 'component', 'WorldComponent'], function(box2d, Component, WorldComponent)
{
  class BodyComponent extends Component
  {
    constructor(entity)
    {
      super(entity);
      this.set("body", WorldComponent.instance.createBody(new box2d.b2BodyDef()));
    }

    get body()
    {
      return this.get("body");
    }

    remove()
    {
      var body = this.body;
      if(super.remove())
      {
        if(body)
        {
          body.GetWorld().DestroyBody(body);
        }
        return true;
      }
      return false;
    }
  }

  return BodyComponent;
});
