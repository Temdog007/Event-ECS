define(function()
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

    toSimpleObject()
    {
      return {
        id : this.id,
        name : this.name,
        enabled : this.enabled
      };
    }
  }

  return EcsObject;
});
