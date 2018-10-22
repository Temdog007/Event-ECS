define(['ecsobject', 'component'], function(EcsObject, Component)
{
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
      this._data = {};
    }

    get(value)
    {
      return this.data[value];
    }

    set(key, value)
    {
      this.data[key] = value;
    }

    setDefault(key, value)
    {
      if(this.get(key) == null && value != null)
      {
        this.set(key, value);
      }
    }

    setDefaults(obj)
    {
      for(var key in obj)
      {
        this.setDefault(key, obj[key]);
      }
    }

    addComponent(componentConstructor)
    {
      if(typeof componentConstructor == "string")
      {
        componentConstructor = require(componentConstructor);
      }

      if(typeof componentConstructor != "function")
      {
        throw "Must pass constructor or required module that returns a consturctor";
      }

      if(this[componentConstructor.name])
      {
        throw "Cannot add duplicate components to one entity: " + component;
      }

      var component = new componentConstructor(this);
      this[componentConstructor.name] = component;
      this.components.push(component);
      this.system.pushEvent('eventAddedComponent', {component : component, entity : this});
      return component;
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

    removeComponent(arg)
    {
      if(typeof arg == "number")
      {
        return this.removeComponentById(arg);
      }
      else if (arg instanceof Component)
      {
        return this.removeComponentByValue(arg);
      }

      throw "Cannot remove component: " + arg;
    }

    removeComponentByValue(comp)
    {
      for(var i = 0; i < this.components.length; ++i)
      {
        var component = this.components[i];
        if(component == comp)
        {
          this.components.splice(i,1);
          console.assert(delete this[component.name], "Failed to delete component from entity");
          return true;
        }
      }
      return false;
    }

    removeComponentById(id)
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
      var ignoreEnabled = args != null && args.ignoreEnabled ? true : false;

      var count = 0;
      for(var i = 0; i < this.components.length; ++i)
      {
        var comp = this.components[i];
        if(ignoreEnabled || comp.enabled)
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

    toSimpleObject()
    {
      var obj = super.toSimpleObject();

      obj.data = {};
      for(var i in this.data)
      {
        var data = this.data[i];
        var type = typeof data;
        if(data == "number" || data == "string" || data == "boolean")
        {
          obj.data[i] = this.data[i];
        }
      }

      obj.components = [];
      for(var i in this.components)
      {
        obj.components.push(this.components[i].toSimpleObject());
      }

      return obj;
    }
  }

  return Entity;
});
