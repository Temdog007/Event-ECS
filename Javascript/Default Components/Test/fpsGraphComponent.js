define(['graphableComponent', 'game'], function(GraphableComponent, Game)
{
  class FpsGraphComponent extends GraphableComponent
  {
    eventUpdate(args)
    {
      var fps = 0.75/(args.dt*0.001) + 0.25 * Game.frameRate;
      this.updateGraph(fps, "FPS: " + Math.floor(fps * 10) / 10, args.dt);
    }
  }

  return FpsGraphComponent;
});
