require.config({
  baseUrl : '../..',
  paths : {
    debugUpdate : "Tests/Debug Test/debugUpdate",
    DrawableComponent : "Default Components/Interfaces/DrawableComponent",
    EmptyComponent : "Tests/Debug Test/emptyComponent",
    EmptyComponent0 : "Tests/Debug Test/emptyComponent0",
    SquareDrawerCompnent : "Tests/Debug Test/SquareDrawerCompnent",
  }
});

require(['game', 'systemlist', 'EmptyComponent', 'EmptyComponent0',
            'SquareDrawerCompnent', 'system', 'debugUpdate'],
function(Game, Systems, EmptyComponent, EmptyComponent0, SquareDrawerCompnent)
{
  var system, entity;
  for(var i = 0; i < 1; ++i)
  {
    system = Systems.addSystem("Test System ");
    // entity = system.createEntity();
    // entity.addComponents([EmptyComponent, EmptyComponent0]);

    entity = system.createEntity();
    // entity.addComponent(EmptyComponent);
    if(i == 0)
    {
      entity.addComponent(SquareDrawerCompnent);
    }
  }
});
