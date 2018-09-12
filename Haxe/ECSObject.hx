import haxe.ds.ObjectMap;

class ECSObject implements IEventHandler
{
  static private var currentID : Int = 1;

  private var my_id : Int;

  public var id(get, null):Int;

  private var map : ObjectMap<Dynamic, Dynamic>;

  @:isVar public var name(get, set): String;

  @:isVar public var enabled(get, set): Bool;

  public function new()
  {
    my_id = currentID++;
    enabled = true;
    name = "ECS Object";
    map = new ObjectMap();
  }

  public function get_id() : Int
  {
    return my_id;
  }

  public function get_name() : String
  {
    return name;
  }

  public function set_name(newName : String)
  {
    return name = newName;
  }

  public function get_enabled() : Bool
  {
    return enabled;
  }

  public function set_enabled(newEnabled : Bool)
  {
    return enabled = newEnabled;
  }

  @:arrayAccess
  public function get(key)
  {
    return map.get(key);
  }

  @:arrayAccess
  public function set(key, value)
  {
    return map.set(key, value);
  }

  public function handleEvent(eventName : String, eventArgs : ObjectMap<Dynamic, Dynamic>) : Int
  {
    throw "'handleEvent' must be overridden in a super class";
  }
}
