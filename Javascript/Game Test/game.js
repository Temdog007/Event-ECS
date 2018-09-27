var canvas = document.getElementById("canvas");
var context = canvas.getContext("2d");
var gl = canvas.getContext("webl") || canvas.getContext("experimental-webgl");
var canvasRect = canvas.getBoundingClientRect();

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

var now;
var start = Date.now();
var updateArgs = {dt : 0};

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
  // drawOrderKeys.forEach(function(value) {console.log(value);});
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

function dispatchDraw(value)
{
  Systems.pushEvent("eventDraw", drawOrders[value]);
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
  drawOrderKeys.forEach(dispatchDraw);
  Systems.flushEvents();

  window.requestAnimationFrame(update);
};

window.requestAnimationFrame(update);
