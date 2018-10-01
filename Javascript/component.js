define(['ecsobject'], function(EcsObject)
{
  class Component extends EcsObject
  {
    constructor(entity)
    {
      if(entity == null)
      {
        console.trace();
        throw "Must have an entity attached to the component";
      }

      super();
      this._entity = entity;
    }

    set(key, value)
    {
      this.entity.set(key, value);
    }

    get(key)
    {
      return this.entity.get(key);
    }

    get entity()
    {
      return this._entity;
    }

    get data()
    {
      return this.entity.data;
    }

    get system()
    {
      return this.entity.system;
    }

    remove()
    {
      return this.entity.removeComponent(this.id);
    }

    dispatchEvent(eventName, args)
    {
      return this.entity.dispatchEvent(eventName, args);
    }
  }

  return Component;
});
