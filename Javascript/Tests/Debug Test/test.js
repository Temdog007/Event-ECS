require.config({
  baseUrl : '../..',
  paths : {
    debugUpdate : "Tests/Debug Test/debugUpdate"
  }
});

require(['game', 'systemlist', 'component', 'system', 'debugUpdate'],
function(Game, Systems, Component)
{
  class EmptyComponent extends Component
  {
    added()
    {
      this.setDefaults({
        msg : "Default Message",
        systemID : this.system.id,
        systemName : this.system.name,
        bool : true,
        time : 0
      });
    }

    eventUpdate(args)
    {
      this.data.time += args.dt;
    }

    eventDebug()
    {
      console.log(this.data.bool + " " + this.data.msg + ": " +
      this.data.systemName + "(" + this.data.systemID + ") at  " + this.data.time);
    }
  }

  class EmptyComponent2 extends EmptyComponent
  {
    eventUpdate(args)
    {
      this.data.time -= args.dt;
    }
  }

  for(var i = 0; i < 5; ++i)
  {
    var system = Systems.addSystem("Test System" + i);
    var entity = system.createEntity();
    entity.addComponents([EmptyComponent, EmptyComponent2]);

    entity = system.createEntity();
    entity.addComponent(EmptyComponent);
  }
});
