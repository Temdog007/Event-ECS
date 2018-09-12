class Entity extends ECSObject
{
  private var system : System;

  public function new(system : System)
  {
    super();
    this.system = system;
  }

  public function addComponent(component : Component)
  {

  }
}
