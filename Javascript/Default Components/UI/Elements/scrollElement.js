class ScrollElement extends Element
{
  constructor(guiComponent, label, pos, parent, values)
  {
    super(guiComponent, label, pos, parent);
    this.values = {min : 0, max : 0, current : 0, step : this.style.unit, axis : 'vertical'};
    for(var key in values)
    {
      this.values[key] = values[key];
    }
  }

  update(dt)
  {
    if(this.withinRect(this.mx, this.my, this.getPosition()) && this.guiComponent.drag)
    {
      this.guiComponent.mousein = this;
    }
  }

  step(step)
  {
    if(step > 0)
    {
      this.values.current = Math.max(this.values.current - this.values.step, this.values.min);
    }
    else if(step < 0)
    {
      this.values.current = Math.min(this.values.current + this.values.step, this.values.max);
    }
    if(this.dragging)
    {
      this.dragging();
    }
  }

  drag(x, y)
  {
    var pos = this.getPosition();
    var hs = this.style.hs;
    if(hs == 'auto')
    {
      if(this.values.axis == 'vertical')
      {
        var h = this.parent ? this.parent.height : pos.height;
        hs = Math.max(4, Math.min(pos.height, pos.height * h / (this.values.max - this.values.min + h)));
      }
      else
      {
        var w = this.parent ? this.parent.width : pos.width;
        hs = Math.max(4, Math.min(pos.width, pos.width * w / (this.values.max - this.values.min + w)));
      }
    }
    if(this.values.axis == 'vertical' && pos.height == hs || this.values.axis != 'vertical' && pos.width == hs)
    {
      this.values.current = 0;
    }
    else
    {
      this.values.current = this.values.min + ((this.values.max - this.values.min) *
        ((this.values.axis == 'vertical' ?
          ((Math.min(Math.max(pos.y, y - Math.floor(hs / 2)), (pos.y + pos.height - hs)) - pos.y) / (pos.height - hs))) :
          ((Math.min(Math.max(pos.x, x - Math.floor(hs / 2)), (pos.x + pos.width - hs)) - pos.x) / (pos.width -hs))));
    }
  }

  rdrag()
  {
    this.drag();
  }

  wheelup()
  {
    if(this.values.axis == 'horizontal')
    {
      this.step(-1);
    }
    else
    {
      this.step(1);
    }
  }

  done()
  {
    this.guiComponent.unfocus();
  }

  draw(pos)
  {
    if(this == this.guiComponent.mousein || this == this.guiComponent.drag || this == this.guiComponent.focus)
    {
      context.fillStyle = this.style.default;
    }
    else
    {
      context.fillStyle = this.style.bg;
    }
    this.rect(pos);

    if(this == this.guiComponent.mousein || this == this.guiComponent.drag || this == this.guiComponent.focus)
    {
      context.fillStyle = this.style.fg;
    }
    else
    {
      context.fillStyle = this.style.hilite;
    }

    var hs = this.style.hs;
    if(hs == 'auto')
    {
      if(this.values.axis == 'vertical')
      {
        var h = this.parent ? this.parent.height : pos.height;
        hs = Math.max(4, Math.min(pos.height, pos.height * h / (this.values.max - htis.values.min + h)));
      }
      else
      {
        var w = this.parent ? this.parent.width : pos.width;
        hs = Math.max(4, Math.min(pos.width, pos.width * w / (this.values.max - this.values.min + w)));
      }
    }

    var handlepos = new Position();
    handlepos.x = this.values.axis == 'horizontal' ?
      Math.min(pos.x, + pos.width - hs,
        Math.max(pos.x, pos.x + ((pos.width - hs) * ((this.values.current - this.values.min) / (this.values.max - this.values.min)))))
          : pos.x;

    handlepos.y = this.values.axis == 'vertical' ?
      Math.max(pos.y, pos.y + ((pos.height - hs) * ((this.values.current - this.values.min) / (this.values.max - this.values.min))))
        : pos.y;

    handlepos.width = this.values.axis == 'horizontal' ? hs : pos.width;
    handlepos.height = this.values.axis == 'vertical' ? hs : pos.height;
    handlepos.radius = pos.radius;

    this.drawShape(handlepos);
    if(this.label)
    {
      context.fillStyle = this.style.labelfg;
      context.fillText(this.label,
        (this.values.axis == 'horizontal' ? pos.x - pos.width : pos.x + pos.width * 0.5),
        (this.values.axis == 'vertical' ? pos.y + pos.height * 0.5 : pos.y + pos.height * 0.5));
    }
  }
}
