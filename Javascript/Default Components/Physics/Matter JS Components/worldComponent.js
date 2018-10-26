define(['component'], function(Component)
{
  var instance;
  class WorldComponent extends Component
  {
    constructor(entity)
    {
      super(entity);

      var world = new Matter.World.create({ x : 0, y : 9.81});
      this.setDefaults({
        engine : new Matter.Engine.create({enabledSleeping : true, world : world}),
        world : world
      });
      instance = this;
    }

    static get instance()
    {
      return instance;
    }

    createBody(options)
    {
      var body = Matter.Body.create(options);
      Matter.World.addBody(this.world, body);
      return body;
    }

    get world()
    {
      return this.get("world");
    }

    get engine()
    {
      return this.get("engine");
    }

    eventUpdate(args)
    {
      Matter.Engine.update(this.engine);
    }
  }

  return WorldComponent;
});
