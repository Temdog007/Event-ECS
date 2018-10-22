require.config({
  baseUrl : '../..',
  paths : {
    DataDisplayerComponent : 'Default Components/Debugging/DataDisplayerComponent'
  }
});

require(['game', 'systemlist', 'component', 'system', 'DataDisplayerComponent'],
function(Game, Systems, Component)
{
  var system = Systems.addSystem("Debugging System");
  var entity = system.createEntity();
  entity.addComponent("DataDisplayerComponent");

  class EmptyComponent extends Component
  {
    added()
    {
      this.setDefaults({
        msg : "Default Message",
        systemID : this.system.id,
        systemName : this.system.name
      });
    }

    eventDebug()
    {
      console.log("Debug Message from " + this.system.name);
    }
  }

  for(var i = 0; i < 5; ++i)
  {
    var system = Systems.addSystem("Test System" + i);
    var entity = system.createEntity();
    entity.addComponent(EmptyComponent);
  }
});
