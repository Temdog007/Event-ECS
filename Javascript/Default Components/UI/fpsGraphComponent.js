class FpsGraphComponent extends GraphableComponent
{
  eventUpdate(args)
  {
    var fps = 0.75/args.dt + 0.25 * frameRate;
    this.updateGraph(fps, "FPS: " + Math.floor(fps * 10) / 10, args.dt);
  }
}
