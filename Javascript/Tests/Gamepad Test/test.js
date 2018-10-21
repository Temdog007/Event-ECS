require.config({
  baseUrl : "../..",
  paths :
  {
    DrawableComponent : "Default COmponents/Interfaces/drawableComponent"
  }
});

require(['game', 'system', 'systemlist', 'DrawableComponent'], function(Game, System, Systems, Component)
{
  function objToString(obj)
  {
    var str = "";
    for(var key in obj)
    {
      str += key + ":" + obj[key] + ", ";
    }
    return str;
  }

  class GamepadComponent extends Component
  {
    constructor(entity)
    {
      super(entity);
      this.context.font = "30px Arial";
      this.set("drawOrder", 0);
    }

    eventDraw()
    {
      this.context.fillStyle = "white";
      this.context.textBaseline = "top";

      var gamepads = navigator.getGamepads();
      for(var i = 0; i < gamepads.length; ++i)
      {
        this.context.fillText(objToString(gamepads[i]), 100, i * 30);
      }
    }

    eventGamepadConnected(args)
    {
      console.log(args);
    }

    eventGamepadDisconnected(args)
    {
      console.log(args);
    }
  }

  var system = Systems.addSystem(new System("Gamepad Test"));
  var entity = system.createEntity();
  entity.addComponent(GamepadComponent);
});
