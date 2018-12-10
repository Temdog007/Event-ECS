define(['ecsobject', 'component'], function(EcsObject, Component)
{
  class Entity extends EcsObject
  {
    constructor(_system)
    {
      super();
      this._system = _system;
      this._components = [];
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

    setValues(table)
    {
      for(var i in table)
      {
        this.set(i, table[i]);
      }
    }

    setDefault(key, value)
    {
      if(typeof key == "object")
      {
        throw "Cannot use object as key";
      }

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
        throw "Cannot add duplicate _components to one entity: " + component;
      }

      var component = new componentConstructor(this);
      this[componentConstructor.name] = component;
      this._components.push(component);
      this.system.pushEvent('eventAddedComponent', {component : component, entity : this});
      return component;
    }

    addComponents(_components)
    {
      if(!Array.isArray(_components))
      {
        throw new TypeError("Need to send array to addComponents");
      }

      var rval = [];
      for(var key in _components)
      {
        rval.push(this.addComponent(_components[key]));
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
      for(var component of this._components)
      {
        if(component == comp)
        {
          if(!component.dead)
          {
            component.dead = true;
            component.remove();
          }
          console.assert(delete this[component.name], "Failed to delete component from entity");
          return true;
        }
      }
      return false;
    }

    removeComponentById(id)
    {
      for(var component of this._components)
      {
        if(component.id == id)
        {
          if(!component.dead)
          {
            component.dead = true;
            component.remove();
          }
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
      this._components = this._components.filter(c => !c.dead);
      return this._components.length;
    }

    forEach(func)
    {
      for(var component of this._components)
      {
        func(component);
      }
    }

    *getComponents()
    {
      for(var component of this._components)
      {
        yield component;
      }
    }

    remove()
    {
      for(var component of this._components)
      {
        this.removeComponentByValue(component);
      }
      return this._system.removeEntity(this);
    }

    dispatchEvent(eventName, args)
    {
      var ignoreEnabled = args != null && args.ignoreEnabled ? true : false;

      var count = 0;
      for(var i = 0; i < this._components.length; ++i)
      {
        var comp = this._components[i];
        if(comp.dead)
        {
          this._components.splice(i--,1);
        }
        else if(ignoreEnabled || comp.enabled)
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
        if(data == "number" || data == "string" || data == "boolean")
        {
          obj.data[i] = this.data[i];
        }
      }

      obj._components = [];
      for(var i in this._components)
      {
        obj._components.push(this._components[i].toSimpleObject());
      }

      return obj;
    }
  }

  return Entity;
});
