define(['ecsevent'], function(ECSEvent)
{
  class SystemList
  {
    constructor()
    {
      this.systems = [];
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
      for(var i = 0; i < this.systems.length; ++i)
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

    pushEvent(eventName, args)
    {
      var ev = new ECSEvent(eventName, args);
      for(var i in this.systems)
      {
        this.systems[i].pushEvent(ev);
      }
    }

    flushEvents()
    {
      var count = 0;
      for(var i in this.systems)
      {
        if(this.systems[i].enabled)
        {
          count += this.systems[i].flushEvents();
        }
      }
      return count;
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
      var results = [];
      for(var i = 0; i < this.systems.length; ++i)
      {
        var sys = this.systems[i];
        results.push(func(sys, args));
      }
      return results;
    }

    get systemCount()
    {
      return this.systems.length;
    }

    toSimpleObject()
    {
      var data = [];

      for(var i in this.systems)
      {
        var system = this.systems[i];
        data.push(system.toSimpleObject());
      }

      return data;
    }
  }

  return new SystemList();
});
