var frames = 0;
var frameRate = 30;

function getFPS()
{
  return frameRate;
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

function addDrawOrder(value, dontSort)
{
  drawOrders[value] = {drawOrder : value};
  if(!dontSort)
  {
    sortDrawOrders();
  }
}

function addDrawOrders(arr)
{
  for(var i = 0; i < arr.length; ++i)
  {
    addDrawOrder(arr[i], true);
  }
  sortDrawOrders();
}

var canvas = document.getElementById("canvas");
var context = canvas.getContext("2d");
var canvasRect = canvas.getBoundingClientRect();

var updateArgs = {dt : 0};

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

function dispatchDraw(value)
{
  Systems.pushEvent("eventDraw", drawOrders[value]);
}

var last = 0;
function update(now)
{
  updateArgs.dt = (now - last) / 1000;
  last = now;

  ++frames;

  Systems.pushEvent("eventUpdate", updateArgs);
  Systems.flushEvents();

  context.clearRect(0, 0, canvas.width, canvas.height);
  drawOrderKeys.forEach(dispatchDraw);
  Systems.flushEvents();

  window.requestAnimationFrame(update);
};

window.requestAnimationFrame(update);
