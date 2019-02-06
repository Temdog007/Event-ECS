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
      this._eventMap = {};
    }

    *[Symbol.iterator]()
    {
      for(var comp of this._components)
      {
        yield comp;
      }
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
        this.setDefaults(key);
      }
      else if(this.get(key) == null && value != null)
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
      
      if(!component instanceof Component)
      {
        throw "Components must extends Component class";
      }

      this[componentConstructor.name] = component;
      this._components.push(component);
      this.system.pushEvent('eventAddedComponent', {component : component, entity : this});
      for(var methodName of getAllMethodNames(component))
      {
        var value = component[methodName];
        if(methodName.startsWith("event"))
        {
          if(this._eventMap[methodName] === undefined)
          {
            this._eventMap[methodName] = [];
          }
          this._eventMap[methodName].push(component);
        }
      }
      return component;
    }

    addComponents(components)
    {
      if(!Array.isArray(components))
      {
        throw new TypeError("Need to send array to addComponents");
      }
      
      var rval = [];
      for(var comp of components)
      {
        rval.push(this.addComponent(comp));
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
      for(var i = 0; i < this._components.length; ++i)
      {
        var component = this._components[i];
        if(component === comp)
        {
          if(!component.dead)
          {
            component.dead = true;
            component.remove();
            this._components.splice(i, 1);
            this._removeEventMap(component);
          }
          console.assert(delete this[component.name], "Failed to delete component from entity");
          return true;
        }
      }
      return false;
    }

    removeComponentById(id)
    {
      for(var i = 0; i < this._components.length; ++i)
      {
        var component = this._components[i];
        if(component.id === id)
        {
          if(!component.dead)
          {
            component.dead = true;
            component.remove();
            this._components.splice(i, 1);
            this._removeEventMap(component);
          }
          console.assert(delete this[component.name], "Failed to delete component from entity");
          return true;
        }
      }
      return false;
    }

    _removeEventMap(component)
    {
      for(var i in this._eventMap)
      {
        var list = this._eventMap[i];
        for(var j = 0; j < list.length; ++j)
        {
          var comp = list[j];
          if(comp === component)
          {
            list.splice(j, 1);
            break;
          }
        }
      }
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
      var list = this._eventMap[eventName];
      if(list === undefined || list.length === 0){return 0;}

      var ignoreEnabled = (args != null && args.ignoreEnabled) ? true : false;
      
      var count = 0;
      for(var component of list)
      {
        if(ignoreEnabled || component.enabled)
        {
          component[eventName].call(component, args);
          ++count;
        }
        if(args != null && args.handled === true)
        {
          break;
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

  function getAllMethodNames(obj)
  {
    var methods = new Set();
    while(obj = Reflect.getPrototypeOf(obj))
    {
      var keys = Reflect.ownKeys(obj);
      keys.forEach(k => methods.add(k));
    }
    return methods;
  }
  return Entity;
});
