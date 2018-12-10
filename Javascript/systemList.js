define(['ecsevent', 'system'], function(ECSEvent, System)
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
        system = new System(system);
      }
      system.Systems = this;

      this.systems.push(system);
      system.systemList = this;
      return system;
    }

    hasSystem(system)
    {
      return this.systems.includes(system);
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
      var ev;
      if(typeof eventName === "string")
      {
        ev = new ECSEvent(eventName, args);
      }
      else if(eventName instanceof ECSEvent)
      {
        ev = eventName;
      }
      else
      {
        throw "Bad event: " + eventName;
      }

      for(var i = 0; i < this.systems.length; ++i)
        {
          this.systems[i].pushEvent(ev);
        }
    }

    flushEvents()
    {
      var count = 0;
      for(var system of this.systems)
      {
        count += system.flushEvents();
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

    find(id)
    {
      for(var system of this.systems)
      {
        if(system.id == id)
        {
          return system;
        }

        for(var entity of system._entities)
        {
          if(entity.id == id)
          {
            return entity;
          }
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

      for(var i = 0; i < this.systems.length; ++i)
      {
        var system = this.systems[i];
        data.push(system.toSimpleObject());
      }

      return data;
    }
  }

  return new SystemList();
});
