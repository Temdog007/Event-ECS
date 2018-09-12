import haxe.ds.StringMap;

class System extends ECSObject
{
  var entities : Array<Entity> ;
  var enabledEntities : Array<Entity> ;
  var registeredEntities : StringMap<Array<Entity -> Component>>;

  public function new(name : String)
  {
      super();
      this.name = name;
      entities = new Array<Entity>();
      enabledEntities = new Array<Entity>();
      registeredEntities = new StringMap<Array<Entity -> Component>>();
  }

  public function registerEntity(name : String, arr : Array<Entity -> Component>)
  {
    var list = registeredEntities.get(name);
    if(list == null)
    {
      list = arr;
    }
    else
    {
      list.concat(arr);
    }

    registeredEntities.set(name, list);
  }

  public function createEntity(?name : String) : Entity
  {
    var entity = new Entity(this);
    entities.push(entity);
    enabledEntities.push(entity);
    if(name != null)
    {
      var components = registeredEntities.get(name);
      if (components == null)
      {
        throw "Failed to get registered entity";
      }
      for (component in components)
      {
        component(entity);
      }
    }
    return entity;
  }

  static function main() : Void{}
}
