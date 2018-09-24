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
    if(this.registeredEntities[name] != null)
    {
      throw {msg : "An entity with this name has already by registered", name : name};
    }

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
      var en = this.entities[i];
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
      return this.findEntitiesByFunction(arg);
    }
    else if(typeof arg == "number")
    {
      return this.findEntitiesByID(arg);
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
    var list;
    if(args != null && args.ignoreEnabled)
    {
      list = this.entities;
    }
    else
    {
      list = this.enabledEntities;
    }

    for(var i = 0; i < list.length; ++i)
    {
      var en = list[i];
      en.dispatchEvent(eventName, args);
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
        entities.push(en);
      }
    }
    this.enabledEntities = entities;
  }
}
