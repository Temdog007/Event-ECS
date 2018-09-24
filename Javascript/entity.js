function contains(obj, value)
{
  for(var key in obj)
  {
    if(key == value || obj[key] == value)
    {
      return true;
    }
  }
  return false;
}

function removeAll(comp)
{
  comp.remove();
}

class Entity extends EcsObject
{
  constructor(_system)
  {
    super();
    this._system = _system;
    this.name = "Entity";
    this.components = [];
  }

  addComponent(component)
  {
    if(this[component.name])
    {
        throw {msg : "Cannot add duplicate components to one entity", component : component.name};
    }

    var comp;
    if(typeof component == "function")
    {
      comp = new component(this);
    }
    else
    {
      comp = component;
    }

    this[component.name] = comp;
    this.components.push(comp);
    this._system.dispatchEvent('eventAddedComponent', {component : comp, entity : this});
    return comp;
  }

  removeComponent(id)
  {
    for(var i = 0; i < this.components.length; ++i)
    {
      if(this.components[i].id == id)
      {
        this.components.splice(i,1);
        return true;
      }
    }
    return false;
  }

  get system()
  {
    return this._system;
  }

  get componentCount()
  {
    return this.components.length;
  }

  forEach(func)
  {
    for(var comp in this.components)
    {
      func(comp);
    }
  }

  remove()
  {
    forEach(removeAll);
    return this._system.removeEntity(this);
  }

  removeComponent(id)
  {
    for(var i = 0; i < this.components.length; ++i)
    {
      if(this.components[i].id == id)
      {
        this.components.splice(i, 1);
        return true;
      }
    }
    return false;
  }

  dispatchEvent(eventName, args)
  {
    if (eventName == "eventValueChanged" && args.id == this.id && contains(args, "enabled"))
    {
      this._system.updateEnabledEntities();
    }

    var count = 0;
    for(var i = 0; i < this.components.length; ++i)
    {
      var comp = this.components[i];
      if(comp.enabled || (args && args.ignoreEnabled))
      {
        var func = comp[eventName];
        if(typeof func === "function")
        {
          func.call(comp, args);
          ++count;
        }
      }
    }
    return count;
  }
}
