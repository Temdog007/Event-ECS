var GetUniqueID = (function ()
{
  var id = 0;
  return function(){return id++;}
})();

class EcsObject
{
  constructor()
  {
    this._name = "ECS Object";
    this._enabled = true;
    this._data = {enabled : true};
    this._id = GetUniqueID();
    this.eventArgs =
    {
        changes : {},
        ignoreEnabled : false,
        id : this._id
    };
    this.setDefault("dispatchEventOnValueChange", false);
  }

  get data()
  {
    return this._data;
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
    this.set("enabled", _enabled, true);
  }

  get name()
  {
    return this._name;
  }

  set name(_name)
  {
    this._name = _name;
  }

  set(_name, value, ignoreEnabled = false)
  {
    var oldValue = this._data[_name];
    if(oldValue == null || _name == "dispatchEventOnValueChange")
    {
      this._data[_name] = value;
    }
    else if (oldValue != value)
    {
      this._data[_name] = value;
      if(this.get("dispatchEventOnValueChange") == true)
      {
        if(Systems.hasEvent("eventValueChanged", this.eventArgs))
        {
          this.eventArgs.ignoreEnabled = this.eventArgs.ignoreEnabled || ignoreEnabled;
          this.eventArgs.changes[_name] = true;
        }
        else
        {
          this.eventArgs.ignoreEnabled = ignoreEnabled || false;
          this.eventArgs.changes = {};
          this.eventArgs.changes[_name] = true;
          Systems.pushEvent("eventValueChanged", this.eventArgs);
        }
      }
    }
  }

  get(_name)
  {
    return this._data[_name];
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
