define(['EmptyComponent'], function(EmptyComponent)
{
  class EmptyComponent0 extends EmptyComponent
  {
    eventUpdate(args)
    {
      this.data.time -= args.dt;
    }
  }

  return EmptyComponent0;
})
