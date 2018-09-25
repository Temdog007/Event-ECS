var canvas = document.getElementById("canvas");
var context = canvas.getContext("2d");

var fps = 30;

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

Systems.drawOrders = [0];
setInterval(function()
{
  ++frames;

  Systems.pushEvent("eventUpdate");
  Systems.flushEvents();

  context.clearRect(0, 0, canvas.width, canvas.height);
  for(var i = 0; i < Systems.drawOrders.length; ++i)
  {
    Systems.pushEvent("eventDraw", {drawOrder : i});
  }
  Systems.flushEvents();

}, 1000 / fps);
