define(['ecsobject', 'entity', 'ecsevent', 'queue'], function(EcsObject, Entity, ECSEvent, Queue)
{
  class System extends EcsObject
  {
    constructor(name)
    {
      super();
      this.name = name;
      this._entities = [];
      this._events = new Queue();
      this._registeredEntities = {};
    }

    *[Symbol.iterator]()
    {
      for(var entity of this._entities)
      {
        yield entity;
      }
    }

    registerEntity(name, args)
    {
      if(this._registeredEntities[name] != null)
      {
        throw {msg : "An entity with this name has already by registered", name : name};
      }

      this._registeredEntities[name] = args;
    }

    registerEntities(args)
    {
      for(var key in args)
      {
        this.registerEntity(key, args[key]);
      }
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
          for(var comp of args)
          {
            en.addComponent(comp);
          }
        }
      }
      return en;
    }

    hasEntity(entity)
    {
      return this._entities.includes(entity);
    }

    removeEntity(entity)
    {
      if(entity.dead){return false;}
      
      for(var en of this._entities)
      {
        if(en == entity || en.id == entity)
        {
          entity.dead = true;
          entity.remove();
          this.pushEvent('eventRemovedEntity', {entity : en, system : this});
          return true;
        }
      }
      return false;
    }

    forEach(func)
    {
      for(var en of this._entities)
      {
        func(en);
      }
    }

    get entityCount()
    {
      this._entities = this._entities.filter(en => !en.dead);
      return this._entities.length;
    }

    findEntitiesByFunction(func)
    {
      var entities = [];
      for(var en of this)
      {
        if(func(en))
        {
          entities.push(en);
        }
      }
      return entities;
    }

    findEntitiesByID(id)
    {
      var entities = [];
      for(var en of this)
      {
        if(en.id == id)
        {
          entities.push(en);
        }
      }
      return entities;
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
      for(var ev in this._events.iterate())
      {
        if(ev === eventName || (ev.name == eventName && ev.args == args))
        {
          return true;
        }
      }
      return false;
    }

    pushEvent(eventName, args)
    {
      if(!this.enabled){return;}
      
      if(eventName instanceof ECSEvent)
      {
        this._events.enqueue(eventName);
      }
      else if(typeof eventName == "string")
      {
        this._events.enqueue(new ECSEvent(eventName, args));
      }
      else
      {
        throw "Cannot push event: " + eventName;
      }
    }

    flushEvents()
    {
      if(this._events.length == 0 || this._entities.length == 0)
      {
        this._events.clear();
        return 0;
      }

      var count = 0;
      var current = this._events.length;
      while(current-- > 0 && this._events.length > 0)
      {
        var ev = this._events.dequeue();
        count += this.dispatchEvent(ev.name, ev.args);
      }
      return count;
    }

    dispatchEvent(eventName, args)
    {
      var ignoreEnabled = (args != null && args.ignoreEnabled) ? true : false;

      var count = 0;
      for(var i = 0; i < this._entities.length; ++i)
      {
        var en = this._entities[i];
        if(en.dead)
        {
          this._entities.splice(i--, 1);
        }
        else if(ignoreEnabled || en.enabled)
        {
          count += en.dispatchEvent(eventName, args);
        }
      }
      return count;
    }

    toSimpleObject()
    {
      var obj = super.toSimpleObject();

      obj.entities = [];
      for(var i = 0; i < this._entities.length; ++i)
      {
        var entity = this.entities[i];
        obj.entities.push(entity.toSimpleObject());
      }

      return obj;
    }
  }

  return System;
});
