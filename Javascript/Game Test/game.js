var canvas = document.getElementById("canvas");
var context = canvas.getContext("2d");
var canvasRect = canvas.getBoundingClientRect();

var frames = 0;
var frameRate = 30;

Systems.getFps = function()
{
  return frameRate;
}

setInterval(function()
{
  frameRate = frames;
  frames = 0;
}, 1000);

var now;
var start = Date.now();
var updateArgs = {dt : 0};

var drawOrders = {[0] : {drawOrder : 0}};

function addDrawOrder(value)
{
  drawOrders[value] = {drawOrder : value};
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
    button = event.which == 3;
  }
  else if ("button" in event)
  {
    button = e.button == 2;
  }
  Systems.pushEvent('eventMouseDown', {which : button});
}

canvas.onmouseup = function(event)
{
  event = event || window.event;

  var button;
  if("which" in event)
  {
    button = event.which == 3;
  }
  else if ("button" in event)
  {
    button = e.button == 2;
  }
  Systems.pushEvent('eventMouseUp', {which : button});
}

canvas.onmouseleave = function(event)
{
  event = event || window.event;
  Systems.pushEvent('eventMouseLeave');
}

canvas.onmouseenter = function(event)
{
  event = event || window.event;
  Systems.pushEvent('eventMouseEnter');
}

function update()
{
  now = Date.now();
  updateArgs.dt = (now - start) / 1000;
  start = Date.now();

  ++frames;

  Systems.pushEvent("eventUpdate", updateArgs);
  Systems.flushEvents();

  context.clearRect(0, 0, canvas.width, canvas.height);
  for(var order in drawOrders)
  {
    Systems.pushEvent("eventDraw", drawOrders[order]);
  }
  Systems.flushEvents();

  window.requestAnimationFrame(update);
};

window.requestAnimationFrame(update);
