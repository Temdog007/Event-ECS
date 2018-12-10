define(['systemlist', 'ecsevent'], function(Systems, ECSEvent)
{
  window.addEventListener("gamepadconnected", function(e)
  {
    Systems.pushEvent("eventGamepadConnected", e.gamepad);
  });

  window.addEventListener("gamepaddisconnected", function(e)
  {
    Systems.pushEvent("eventGamepadDisconnected", e.gamepad);
  });

  var Game = {}

  CanvasRenderingContext2D.prototype.__defineGetter__('width', function(){return this.canvas.width;});
  CanvasRenderingContext2D.prototype.__defineGetter__('height', function(){return this.canvas.height;});

  var canvas = document.getElementById("canvas");
  var context = canvas.getContext("2d");
  var canvasRect = canvas.getBoundingClientRect();

  function resize()
  {
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
    canvasRect = canvas.getBoundingClientRect();
    Systems.pushEvent('eventResize');
  }
  window.addEventListener("resize", resize);
  resize();

  var layers = {};
  var layersSorted;

  Game.getLayer = function(x)
  {
    return layers[x];
  }

  class Layer
  {
    constructor(id)
    {
      this.canvas = document.createElement("canvas");
      this._id = id;
      this.context = this.canvas.getContext("2d");
      this.rect = this.canvas.getBoundingClientRect();
      this.dontClear = false;
      this.resize = Layer.resize.bind(this);
      window.addEventListener("resize", this.resize);
      this.resize();
    }

    get id()
    {
      return this._id;
    }

    clear()
    {
      var rect = this.clearRect;
      if(rect)
      {
        this.context.clearRect(rect.x, rect.y, rect.width, rect.height);
      }
      else
      {
        this.context.clearRect(0, 0, this.width, this.height);
      }
    }

    static resize()
    {
      this.width = window.innerWidth;
      this.height = window.innerHeight;
      this.rect = this.canvas.getBoundingClientRect();
    }

    get width()
    {
      return this.canvas.width;
    }

    set width(value)
    {
      this.canvas.width = value;
    }

    get height()
    {
      return this.canvas.height;
    }

    set height(value)
    {
      this.canvas.height = value;
    }
  }

  function sortLayers()
  {
    layersSorted = Object.keys(layers).sort();
  }

  Game.addLayer = function(value, dontSort)
  {
    if(!layers[value])
    {

      layers[value] = new Layer(value);
      if(dontSort !== true)
      {
        sortLayers();
      }
      Systems.pushEvent("eventLayerAdded", layers[value]);
    }
    return layers[value];
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
    Systems.pushEvent("eventMouseMoved", {
      x : (event.clientX - canvasRect.left) / (canvasRect.right - canvasRect.left) * canvas.width, 
      y : (event.clientY - canvasRect.top) / (canvasRect.bottom - canvasRect.top) * canvas.height, 
      event : event
    });
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

    Systems.pushEvent('eventMouseDown', {
      x : (event.clientX - canvasRect.left) / (canvasRect.right - canvasRect.left) * canvas.width, 
      y : (event.clientY - canvasRect.top) / (canvasRect.bottom - canvasRect.top) * canvas.height, 
      button : event.button, 
      event : event,
      buttonName : button});
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

    Systems.pushEvent('eventMouseUp', {
      x : (event.clientX - canvasRect.left) / (canvasRect.right - canvasRect.left) * canvas.width, 
      y : (event.clientY - canvasRect.top) / (canvasRect.bottom - canvasRect.top) * canvas.height, 
      button : event.button, 
      event : event, 
      buttonName : button});
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

  var updateArgs = {dt : 0};
  var frames = 0;
  var frameRate = 0;
  var last = 0;
  var startTime = Date.now();
  setInterval(function()
  {
    frameRate = frames;
    frames = 0;
  }, 1000);

  var updateEvent = new ECSEvent("eventUpdate", updateArgs);
  var drawEvent = new ECSEvent("eventDraw");

  function updateContext()
  {
    context.clearRect(0, 0, canvas.width, canvas.height);
    context.save();
    for(var i = 0; i < layersSorted.length; ++i)
    {
      var layer = layers[layersSorted[i]];
      if(!layer){
        continue;
      }
      context.drawImage(layer.canvas, 0, 0, layer.width, layer.height,
                            0, 0, canvas.width, canvas.height);
    }
    context.restore();
  }

  function addUpdateEvents()
  {
    for(var i = 0; i < layersSorted.length; ++i)
    {
      var layer = layers[layersSorted[i]];
      if(!layer || layer.dontClear){
        continue;
      }
      layer.clear();
    }

    Systems.pushEvent(updateEvent);
    Systems.pushEvent(drawEvent);
    Systems.flushEvents();

    setTimeout(updateContext, 0);
  }

  Object.defineProperties(Game, {
    dt : {
      get : function(){return updateArgs.dt;}
    },
    frames : {
      get : function(){return frames;}
    },
    frameRate : {
      get : function(){ return frameRate;}
    },
    time : {
      get : function() { return Date.now() - startTime;}
    }
  });

  function update(now)
  {
    ++frames;
    updateArgs.dt = now - last;
    last = now;
    setTimeout(addUpdateEvents, 0);
    window.requestAnimationFrame(update);
  };

  window.requestAnimationFrame(update);

  return Game;
});
