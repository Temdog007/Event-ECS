class System extends EcsObject
{
  constructor(name, systemList)
  {
    super();
    this.name = name || "System";
    this.systemList = systemList;
    this.entities = [];
    this.enabledEntities = [];
    this.registeredEntities = {};
  }

  registerEntity(name, args)
  {
    this.registeredEntities[name] = args;
  }

  createEntity(name)
  {
    var en = new Entity(this);
    this.entities.push(en);
    this.enabledEntities.push(en);
    if(name)
    {
      var args = this.registeredEntities[name];
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
    for(var i = 0; i < this.entities.length; ++i)
    {
      if(en == entity || en.id == entity)
      {
        this.dispatchEvent('eventRemovingEntity', {entity : en, system : this});
        this.entities.splice(i, 1);
        this.dispatchEvent('eventRemovedEntity', {entity : en, system : this});
        this.updateEnabledEntities();
        return true;
      }
    }
    return false;
  }

  forEach(func)
  {
    for(var i = 0; i < this.entities.length; ++i)
    {
      func(this.entities[i]);
    }
  }

  get entityCount()
  {
    return this.entities.length;
  }

  findEntitiesByFunction(func)
  {
    var entities = [];
    for(var i = 0; i < this.entities.length; ++i)
    {
      var en = this.entities[i];
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
    for(var i = 0; i < this.entities.length; ++i)
    {
      var en = this.entities[i];
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
      return findEntitiesByFunction(arg);
    }
    else if(typeof arg == "number")
    {
      return findEntitiesByID(arg);
    }
  }

  broadcastEvent(eventName, args)
  {
    if(this.systemList != null)
    {
      this.systemList.pushEvent(eventName, args);
    }
  }

  dispatchEvent(eventName, args)
  {
    if(args && args.ignoreEnabled)
    {
      for(var i = 0; i < this.entities.length; ++i)
      {
        var en = this.entities[i];
        en.dispatchEvent(eventName, args);
      }
    }
    else
    {
      for(var i = 0; i < this.entities.length; ++i)
      {
        var en = this.entities[i];
        en.dispatchEvent(eventName, args);
      }
    }
  }

  updateEnabledEntities()
  {
    var entities = [];
    for(var i = 0; i < this.entities.length; ++i)
    {
      var en = this.entities[i];
      if(en.enabled)
      {
        en.push(en);
      }
    }
    this.enabledEntities = entities;
  }
}
