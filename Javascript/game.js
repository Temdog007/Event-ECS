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

  var drawOrders = {[0] : {drawOrder : 0}};
  var drawOrderKeys = [0];

  function sortDrawOrders()
  {
    drawOrderKeys = [];
    for(var order in drawOrders)
    {
      drawOrderKeys.push(order);
    }
    drawOrderKeys.sort();
  }

  Game.addDrawOrder = function(value, dontSort)
  {
    drawOrders[value] = {drawOrder : value};
    if(!(dontSort == true))
    {
      sortDrawOrders();
    }
  }

  Game.addDrawOrders = function(arr)
  {
    for(var i = 0; i < arr.length; ++i)
    {
      this.addDrawOrder(arr[i], true);
    }
    sortDrawOrders();
  }

  Game.canvas = document.getElementById("canvas");
  Game.context = canvas.getContext("2d");
  Game.canvasRect = canvas.getBoundingClientRect();

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
    Systems.pushEvent("eventMouseMoved", {x : event.clientX - Game.canvasRect.left, y : event.clientY - Game.canvasRect.top});
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

    Systems.pushEvent('eventMouseDown', {x : event.clientX - Game.canvasRect.left, y : event.clientY - Game.canvasRect.top, buttonName : button});
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

    Systems.pushEvent('eventMouseUp', {x : event.clientX - Game.canvasRect.left, y : event.clientY - Game.canvasRect.top, buttonName : button});
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

  function dispatchDraw(value)
  {
    Systems.pushEvent("eventDraw", drawOrders[value]);
  }

  function addUpdateEvents()
  {
    Systems.pushEvent("eventUpdate", updateArgs);
    Systems.flushEvents();

    Game.context.clearRect(0, 0, canvas.width, canvas.height);
    drawOrderKeys.forEach(dispatchDraw);
    Systems.flushEvents();
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
