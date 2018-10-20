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
      this._data = {enabled : true};
    }

    addComponent(component)
    {
      if(this[component.name])
      {
        console.trace();
        throw {msg : "Cannot add duplicate components to one entity", component : component};
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

    addComponents(components)
    {
      if(!Array.isArray(components))
      {
        throw new TypeError("Need to send array to addComponents");
      }

      var rval = [];
      for(var key in components)
      {
        rval.push(this.addComponent(components[key]));
      }
      return rval;
    }

    removeComponent(id)
    {
      for(var i = 0; i < this.components.length; ++i)
      {
        var component = this.components[i];
        if(component.id == id)
        {
          this.components.splice(i,1);
          console.assert(delete this[component.name], "Failed to delete component from entity");
          return true;
        }
      }
      return false;
    }

    get data()
    {
      return this._data;
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
