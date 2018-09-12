import haxe.ds.ObjectMap;

interface IEventHandler
{
  public function handleEvent(eventName : String, eventArgs : ObjectMap<Dynamic, Dynamic>) : Int;
}
