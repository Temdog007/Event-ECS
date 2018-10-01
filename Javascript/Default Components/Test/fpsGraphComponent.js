define(['graphableComponent', 'game'], function(GraphableComponent, Game)
{
  class FpsGraphComponent extends GraphableComponent
  {
    eventUpdate(args)
    {
      var fps = 0.75/args.dt + 0.25 * Game.getFPS();
      this.updateGraph(fps, "FPS: " + Math.floor(fps * 10) / 10, args.dt);
    }
  }

  return FpsGraphComponent;
});
