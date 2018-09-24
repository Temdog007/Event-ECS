var temp;

class Component extends EcsObject
{
  constructor(entity)
  {
    if(entity == null)
    {
      console.trace();
      throw "Must have an entity attached to the component";
    }

    temp = entity;
    super();
    this._entity = entity;
    temp = null;
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
    if(this._entity == null)
    {
      return temp;
    }
    else
    {
      return this._entity;
    }
  }

  get data()
  {
    return this.entity.data;
  }

  remove()
  {
    this.entity.removeComponent(this.id);
  }

  dispatchEvent(eventName, args)
  {
    this.entity.dispatchEvent(eventName, args);
  }
}
