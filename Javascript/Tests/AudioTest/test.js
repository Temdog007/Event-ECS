require.config({
  baseUrl : '../../'
})

require(
[
  'component', 'systemlist', 'game', 'system'
],
function(Component, Systems, _, System)
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

  var system = Systems.addSystem(new System("Test"));
  var entity = system.createEntity();
  entity.addComponent(AudioComponent);
});
