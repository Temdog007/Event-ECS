define(['ecsevent'], function(ECSEvent)
{
  class SystemList
  {
    constructor()
    {
      this.systems = [];
      this.events = [];
    }

    addSystem(system)
    {
      if(typeof system == "string")
      {
        var System = require("system");
        system = new System(system);
      }
      this.systems.push(system);
      system.systemList = this;
      return system;
    }

    removeSystem(system)
    {
      for(var i = 0; i < this.systems; ++i)
      {
        if(this.systems[i].id == system.id)
        {
          this.systems.splice(i, 1);
          return true;
        }
      }
      return false;
    }

    removeAllSystems()
    {
      this.systems = [];
    }

    hasEvent(eventName, args)
    {
      for(var i = 0; i < this.events.length; ++i)
      {
        var ev = this.events[i];
        if(ev.name == eventName && ev.args == args)
        {
          return true;
        }
      }
      return false;
    }

    pushEvent(eventName, args)
    {
      this.events.push(new ECSEvent(eventName, args));
    }

    flushEvents()
    {
      var count = this.events.length;
      var current = 0;
      while(current < count && this.events.length > 0)
      {
        var ev = this.events.shift();
        for(var i = 0; i < this.systems.length; ++i)
        {
          var sys = this.systems[i];
          if(sys.enabled || (ev.args != null && ev.args.ignoreEnabled))
          {
            sys.dispatchEvent(ev.name, ev.args);
          }
        }
        ++current;
      }
    }

    getSystem(value)
    {
      for(var i = 0; i < this.systems.length; ++i)
      {
        var sys = this.systems[i];
        if(sys.id == value || sys.name == value)
        {
          return sys;
        }
      }
    }

    forEachSystem(func, args)
    {
      for(var i = 0; i < this.systems.length; ++i)
      {
        var sys = this.systems[i];
        func(sys, args);
      }
    }

    get count()
    {
      return this.systems.length;
    }
  }

  return new SystemList();
});
