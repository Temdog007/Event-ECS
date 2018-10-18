require.config({
  baseUrl : '../../',
  paths :
  {
    imageDrawerComponent : 'Default Components/Drawing/imageDrawerComponent',
    drawableComponent : 'Default Components/Interfaces/drawableComponent',
    graphableComponent : 'Default Components/Interfaces/graphableComponent',
    eventTesterComponent : 'Default Components/Test/eventTesterComponent',
    coroutineTesterComponent : 'Default Components/Test/coroutineTesterComponent',
    fpsDisplayerComponent : 'Default Components/Test/fpsDisplayerComponent',
    logTestComponent : 'Default Components/Test/logTestComponent',
    fpsGraphComponent : 'Default Components/Test/fpsGraphComponent',
    game : 'Tests/game'
  }
})

require(
[
  'component', 'system', 'game'
],
function(Component, System, Game)
{
  class AudioComponent extends Component
  {
    constructor(entity)
    {
      super(entity);

      this.audio = document.createElement("audio");
      this.audio.src = "test.wav";
      console.log(this.audio.canPlayType("audio/wav"));
      this.audio.play();
    }
  }

  var Systems = Component.Systems;

  var system = Systems.addSystem(new System("Test"));
  var entity = system.createEntity();
  entity.addComponent(AudioComponent);
});
