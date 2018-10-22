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
        systemName : this.system.name,
        bool : true
      });
    }

    eventDebug()
    {
      console.log(this.data.bool + " " + this.data.msg + ": " + 
      this.data.systemName + "(" + this.data.systemID + ") at  " + Date.now());
    }
  }

  for(var i = 0; i < 5; ++i)
  {
    var system = Systems.addSystem("Test System" + i);
    var entity = system.createEntity();
    entity.addComponent(EmptyComponent);
  }
});
