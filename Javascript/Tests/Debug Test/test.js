require.config({
  baseUrl : '../..',
  paths : {
    debugUpdate : "Tests/Debug Test/debugUpdate",
    DrawableComponent : "Default Components/Interfaces/DrawableComponent",
    EmptyComponent : "Tests/Debug Test/emptyComponent",
    EmptyComponent0 : "Tests/Debug Test/emptyComponent0",
    EmptyComponent00 : "Tests/Debug Test/emptyComponent00",
  }
});

require(['game', 'systemlist', 'EmptyComponent', 'EmptyComponent0',
            'EmptyComponent00', 'system', 'debugUpdate'],
function(Game, Systems, EmptyComponent, EmptyComponent0, EmptyComponent00)
{
  for(var i = 0; i < 5; ++i)
  {
    var system = Systems.addSystem("Test System" + i);
    var entity = system.createEntity();
    entity.addComponents([EmptyComponent, EmptyComponent0]);

    entity = system.createEntity();
    entity.addComponent(EmptyComponent);
    if(i == 0)
    {
      entity.addComponent(EmptyComponent00);
    }
  }
});
