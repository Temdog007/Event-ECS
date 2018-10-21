define(['ecsobject'], function(EcsObject)
{
  var Systems = EcsObject.Systems;

  window.addEventListener("gamepadconnected", function(e)
  {
    Systems.pushEvent("eventGamepadConnected", e.gamepad);
  });

  window.addEventListener("gamepaddisconnected", function(e)
  {
    Systems.pushEvent("eventGamepadDisconnected", e.gamepad);
  });

  var frames = 0;
  var frameRate = 30;
  var Game = {}

  var startTime = Date.now();

  Game.getFPS = function()
  {
    return frameRate;
  }

  Game.getStartTime = function()
  {
    return startTime;
  }

  setInterval(function()
  {
    frameRate = frames;
    frames = 0;
  }, 1000);

  var canvas = document.getElementById("canvas");
  var context = canvas.getContext("2d");
  var canvasRect = canvas.getBoundingClientRect();

  var layers = {};
  var layersSorted;

  Game.getLayer = function(x)
  {
    return layers[x];
  }

  class Layer
  {
    constructor()
    {
      this.canvas = document.createElement("canvas");
      this.context = this.canvas.getContext("2d");
      this.rect = this.canvas.getBoundingClientRect();
      this.resize();
      var f = this;
      canvas.addEventListener("onresize", function() {f.resize();});
    }

    resize()
    {
      this.canvas.width = canvas.width;
      this.canvas.height = canvas.height;
    }

    get width()
    {
      return this.canvas.width;
    }

    get height()
    {
      return this.canvas.height;
    }
  }

  function sortLayers()
  {
    layersSorted = Object.keys(layers).sort();
  }

  Game.addLayer = function(value, dontSort)
  {
    if(layers[value]){return;}

    layers[value] = new Layer();
    if(!(dontSort == true))
    {
      sortLayers();
    }
  }

  Game.addLayers = function(arr)
  {
    if(!Array.isArray(arr))
    {
      throw "Must add an array of numbers";
    }

    for(var i = 0; i < arr.length; ++i)
    {
      this.addLayer(arr[i], true);
    }
    sortLayers();
  }

  Game.addLayer(0);

  canvas.oncontextmenu = function(e)
  {
    return false;
  };

  canvas.onkeydown = function(event)
  {
    event = event || window.event;
    Systems.pushEvent("eventKeyDown", event)
  }

  canvas.onkeyup = function(event)
  {
    event = event || window.event;
    Systems.pushEvent("eventKeyUp", event)
  }

  canvas.onmousemove = function(event)
  {
    event = event || window.event;
    Systems.pushEvent("eventMouseMoved", {x : event.clientX - canvasRect.left, y : event.clientY - canvasRect.top});
  }

  canvas.onmousedown = function(event)
  {
    event = event || window.event;

    var button;
    if("which" in event)
    {
      switch(event.which)
      {
        case 1:
          button = "left";
          break;
        case 2:
          button = "middle";
          break;
        case 3:
          button = "right";
          break;
        default:
          button = "unknown";
          break;
      }
    }
    else if ("button" in event)
    {
      switch(event.button)
      {
        case 0:
          button = "left";
          break;
        case 1:
          button = "middle";
          break;
        case 2:
          button = "right";
          break;
        default:
          button = "unknown";
          break;
      }
    }

    Systems.pushEvent('eventMouseDown', {x : event.clientX - canvasRect.left, y : event.clientY - canvasRect.top, buttonName : button});
  }

  canvas.onmouseup = function(event)
  {
    event = event || window.event;

    var button;
    if("which" in event)
    {
      switch(event.which)
      {
        case 1:
          button = "left";
          break;
        case 2:
          button = "middle";
          break;
        case 3:
          button = "right";
          break;
        default:
          button = "unknown";
          break;
      }
    }
    else if ("button" in event)
    {
      switch(event.button)
      {
        case 0:
          button = "left";
          break;
        case 1:
          button = "middle";
          break;
        case 2:
          button = "right";
          break;
        default:
          button = "unknown";
          break;
      }
    }

    Systems.pushEvent('eventMouseUp', {x : event.clientX - canvasRect.left, y : event.clientY - canvasRect.top, buttonName : button});
  }

  canvas.onmouseleave = function(event)
  {
    event = event || window.event;
    Systems.pushEvent('eventMouseLeave', event);
  }

  canvas.onmouseenter = function(event)
  {
    event = event || window.event;
    Systems.pushEvent('eventMouseEnter', event);
  }

  canvas.onwheel = function(event)
  {
    event = event || window.event;
    Systems.pushEvent('eventMouseWheel', event);
  }

  function addUpdateEvents()
  {
    context.clearRect(0, 0, canvas.width, canvas.height);
    for(var i in layersSorted)
    {
      var layer = layers[layersSorted[i]];
      if(!layer){
        continue;
      }
      layer.context.clearRect(0, 0, layer.width, layer.height);
    }

    Systems.pushEvent("eventUpdate", updateArgs);
    Systems.pushEvent("eventDraw");
    Systems.flushEvents();

    context.save();
    for(var i in layersSorted)
    {
      var layer = layers[layersSorted[i]];
      if(!layer){
        continue;
      }
      context.drawImage(layer.canvas, 0, 0);
    }
    context.restore();
  }

  var updateArgs = {dt : 0};
  var last = 0;

  Object.defineProperty(Game, 'dt', {
    get : function(){return updateArgs.dt;}
  });

  function update(now)
  {
    updateArgs.dt = (now - last) / 1000;
    last = now;

    ++frames;

    if(Game.safeMode)
    {
      try
      {
        addUpdateEvents();
      }
      catch(e)
      {
        console.log(e);
      }
    }
    else
    {
      addUpdateEvents();
    }

    window.requestAnimationFrame(update);
  };

  window.requestAnimationFrame(update);

  return Game;
});
