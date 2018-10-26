define(['component'], function(Component)
{
  var instance;
  class WorldComponent extends Component
  {
    constructor(entity)
    {
      super(entity);

      this.setDefaults({
        step : 1 / 60,
        positionIterations : 2,
        velocityIterations : 3,
        world : new b2World(new b2Vec2(0, 9.81))
      });
      instance = this;
    }

    static get instance()
    {
      return instance;
    }

    createBody(bodyDef)
    {
      return this.world.CreateBody(bodyDef);
    }

    get world()
    {
      return this.get("world");
    }

    eventUpdate(args)
    {
      this.world.Step(this.get("step"), this.get("velocityIterations"), this.get("positionIterations"));
    }
  }

  return WorldComponent;
});
