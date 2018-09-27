class GraphableComponent extends DrawableComponent
{
  constructor(entity)
  {
    super(entity);

    var vals = [];
    for(var i = 0; i < 25; ++i)
    {
      vals.push(i);
    }

    this.setDefaults({
      x : 0,
      y : 0,
      width : 100,
      height : 100,
      draggable : true,
      delay : 0.5,
      vals : vals,
      vmax : 0,
      currentTime : 0,
      label : "graph",
      dx : 0,
      dy : 0,
      lineWidth : 1,
      isDown : false,
      font : "16px Arial",
      lineColor : "red",
      fontColor : "black"
    })
  }

  updateGraph(val, label, dt)
  {
    var data = this.data;
    data.currentTime += dt;

    if(data.draggable)
    {
      if(data.isDown)
      {
        data.x = data.mx - data.dx;
        data.y = data.my - data.dy;
      }
      else
      {
        data.dx = data.mx - data.x;
        data.dy = data.my - data.y;
      }
    }

    while(data.currentTime >= data.delay)
    {
      data.currentTime -= data.delay;

      data.vals.shift();
      data.vals.push(val);

      var max = 0;
      for(var i = 0; i < data.vals.length; ++i)
      {
        var v = data.vals[i];
        if(v > max){ max = v;}
      }

      data.vmax = max;
      data.label = label;
    }
  }

  doDraw(args)
  {
    var data = this.data;

    var maxval = Math.ceil(data.vmax / 10) * 10 + 20;
    var len = data.vals.length;
    var step = data.width / len;

    context.strokeStyle = data.lineColor;
    context.lineWidth = data.lineWidth;
    context.beginPath();
    for(var i = 1; i < len; ++i)
    {
      var a = data.vals[i-1];
      var b = data.vals[i];
      context.moveTo(step * (i-2) + data.x, data.height * (-a/maxval + 1) + data.y);
      context.lineTo(step * (i-1) + data.x, data.height * (-b/maxval + 1) + data.y);
    }
    context.stroke();

    context.font = data.font;
    context.fillStyle = data.fontColor;
    context.textBaseline = "top";
    context.fillText(data.label, data.x, data.height + data.y);
  }

  eventMouseDown(args)
  {
    var data = this.data;
    data.isDown = data.isDown || (data.x < data.mx && data.mx < data.x + data.width
      && data.y < data.my && data.my < data.y + data.height);
  }

  eventMouseUp(args)
  {
    this.data.isDown = false;
  }

  eventMouseLeave(args)
  {
    this.data.isDown = false;
  }

  eventMouseMoved(args)
  {
    this.data.mx = args.x;
    this.data.my = args.y;
  }
}
