var canvas = document.getElementById("canvas");
var context = canvas.getContext("2d");

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
