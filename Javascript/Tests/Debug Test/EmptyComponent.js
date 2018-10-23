define(['component'], function(Component)
{
  class EmptyComponent extends Component
  {
    added()
    {
      this.setDefaults({
        msg : "Default Message",
        systemID : this.system.id,
        systemName : this.system.name,
        bool : true,
        time : 0
      });
    }

    eventUpdate(args)
    {
      this.data.time += args.dt;
    }

    eventDebug()
    {
      console.log(this.data.bool + " " + this.data.msg + ": " +
      this.data.systemName + "(" + this.data.systemID + ") at  " + this.data.time);
    }
  }

  return EmptyComponent;
})
