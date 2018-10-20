define(['systemlist'], function(Systems)
{
  var GetUniqueID = (function ()
  {
    var id = 0;
    return function(){return id++;}
  })();

  class EcsObject
  {
    constructor()
    {
      this._enabled = true;
      this._id = GetUniqueID();
    }

    static get Systems()
    {
      return Systems;
    }

    get id()
    {
      return this._id;
    }

    get enabled()
    {
      return this._enabled;
    }

    set enabled(_enabled)
    {
      this._enabled = _enabled;
    }

    get name()
    {
      return this._name || this.constructor.name;
    }

    set name(value)
    {
      this._name = value;
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
  }

  return EcsObject;
});
