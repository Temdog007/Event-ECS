require.config({
  baseUrl : '../../'
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
