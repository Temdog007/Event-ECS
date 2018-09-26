class CoroutineTesterComponent extends Component
{
  constructor(entity)
  {
    super(entity);
    this.scheduler = new CoroutineScheduler();
    this.scheduler.addRoutine(function*()
    {
      for(var i = 0; i < 100; ++i)
      {
        console.log("%c" + i, "color:red");
        yield 1;
      }
    });
  }

  eventUpdate(step)
  {
    this.scheduler.update(step.dt);
  }
}
