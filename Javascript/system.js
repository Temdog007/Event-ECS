define(['ecsobject', 'entity', 'ecsevent'], function(EcsObject, Entity, ECSEvent)
{
  class System extends EcsObject
  {
    constructor(name)
    {
      super();
      this.name = name;
      this._entities = [];
      this._events = [];
      this._registeredEntities = {};
    }

    registerEntity(name, args)
    {
      if(this._registeredEntities[name] != null)
      {
        throw {msg : "An entity with this name has already by registered", name : name};
      }

      this._registeredEntities[name] = args;
    }

    createEntity(name)
    {
      if(name && typeof this._registeredEntities[name] != "function"
              && !Array.isArray(this._registeredEntities[name]))
      {
        throw "Invalid entity name " + name;
      }

      var en = new Entity(this);
      this._entities.push(en);
      if(name)
      {
        var args = this._registeredEntities[name];
        if(typeof args === "function")
        {
          en.addComponent(args);
        }
        else if(Array.isArray(args))
        {
          for(var i = 0; i < args.length; ++i)
          {
            en.addComponent(args[i]);
          }
        }
      }
      return en;
    }

    removeEntity(entity)
    {
      for(var i = 0; i < this._entities.length; ++i)
      {
        var en = this._entities[i];
        if(en == entity || en.id == entity)
        {
          this._entities.splice(i, 1);
          this.pushEvent('eventRemovedEntity', {entity : en, system : this});
          return true;
        }
      }
      return false;
    }

    forEach(func)
    {
      for(var i = 0; i < this._entities.length; ++i)
      {
        func(this._entities[i]);
      }
    }

    get entityCount()
    {
      return this._entities.length;
    }

    findEntitiesByFunction(func)
    {
      var _entities = [];
      for(var i = 0; i < this._entities.length; ++i)
      {
        var en = this._entities[i];
        if(func(en))
        {
          _entities.push(en);
        }
      }
      return _entities;
    }

    findEntitiesByID(id)
    {
      var _entities = [];
      for(var i = 0; i < this._entities.length; ++i)
      {
        var en = this._entities[i];
        if(en.id == id)
        {
          _entities.push(en);
        }
      }
      return _entities;
    }

    findEntities(arg)
    {
      if(typeof arg == "function")
      {
        return this.findEntitiesByFunction(arg);
      }
      else if(typeof arg == "number")
      {
        return this.findEntitiesByID(arg);
      }
      throw "Must have a number or function passed to findEntities. Got: " + typeof arg;
    }

    hasEvent(eventName, args)
    {
      for(var i = 0; i < this._events.length; ++i)
      {
        var ev = this._events[i];
        if(ev === eventName || (ev.name == eventName && ev.args == args))
        {
          return true;
        }
      }
      return false;
    }

    pushEvent(eventName, args)
    {
      if(eventName instanceof ECSEvent)
      {
        this._events.push(eventName);
      }
      else if(typeof eventName == "string")
      {
        this._events.push(new ECSEvent(eventName, args));
      }
      else
      {
        throw "Cannot push event: " + eventName;
      }
    }

    flushEvents()
    {
      if(this._events.length == 0 || this._entities.length == 0){return 0;}

      var count = 0;
      var current = this._events.length;
      while(current-- > 0 && this._events.length > 0)
      {
        var ev = this._events.shift();
        count += this.dispatchEvent(ev.name, ev.args);
      }
      return count;
    }

    dispatchEvent(eventName, args)
    {
      var ignoreEnabled = args != null && args.ignoreEnabled ? true : false;

      var count = 0;
      for(var i = 0; i < this._entities.length; ++i)
      {
        var en = this._entities[i];
        if(ignoreEnabled || en.enabled)
        {
          count += en.dispatchEvent(eventName, args);
        }
      }
      return count;
    }
  }

  return System;
});
