define(['ecsobject'], function(EcsObject)
{
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

    set enabled(enabled)
    {
      super.enabled = enabled;
      this.system.updateEnabledEntities();
    }

    get enabled()
    {
      return super.enabled;
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
      for(var i = 0; i < this.components.length; ++i)
      {
        var comp = this.components[i];
        func(comp);
      }
    }

    remove()
    {
      this.forEach(removeAll);
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

  return Entity;
});
