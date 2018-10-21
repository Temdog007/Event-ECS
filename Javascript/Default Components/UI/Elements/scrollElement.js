define(['uiElement', 'game', 'position'],
function(UIElement, Game, Position)
{
  class ScrollElement extends UIElement
  {
    constructor(label, pos, parent, values)
    {
      super(label, pos, parent);
      this.values = {min : 0, max : 0, current : 0, step : this.style.unit, axis : 'vertical'};
      for(var key in values)
      {
        this.values[key] = values[key];
      }
    }

    get min()
    {
      return this.values.min;
    }

    set min(value)
    {
      this.values.min = value;
    }

    get max()
    {
      return this.values.max;
    }

    set max(value)
    {
      this.values.max = value;
    }

    get current()
    {
      return this.values.current;
    }

    set current(value)
    {
      this.values.current = value;
    }

    get axis()
    {
      return this.values.axis;
    }

    set axis(value)
    {
      this.values.axis = value;
    }

    update(dt)
    {
      if(UIElement.withinRect(this.mx, this.my, this.getPosition()) && !UIElement.guiComponent.drag)
      {
        UIElement.guiComponent.mousein = this;
      }
    }

    step(step)
    {
      if(step > 0)
      {
        this.current = Math.max(this.current - this.values.step, this.min);
      }
      else if(step < 0)
      {
        this.current = Math.min(this.current + this.values.step, this.max);
      }
      if(this.dragging)
      {
        this.dragging();
      }
    }

    drag(x, y)
    {
      var value = this.current;
      var pos = this.getPosition();
      var hs = this.hs;
      if(hs == 'auto')
      {
        if(this.axis == 'vertical')
        {
          var h = this.parent ? this.parent.height : pos.height;
          hs = Math.max(this.unit * 0.25, Math.min(pos.height, pos.height * h / (this.max - this.min + h)));
        }
        else
        {
          var w = this.parent ? this.parent.width : pos.width;
          hs = Math.max(this.unit * 0.25, Math.min(pos.width, pos.width * w / (this.max - this.min + w)));
        }
      }

      if(this.axis == 'vertical' && pos.height == hs || this.axis != 'vertical' && pos.width == hs)
      {
        this.current = 0;
      }
      else
      {
        this.current = this.min + ((this.max - this.min) *
          ((this.axis == 'vertical' ?
            ((Math.min(Math.max(pos.y, y - Math.floor(hs / 2)), (pos.y + pos.height - hs)) - pos.y) / (pos.height - hs)) :
            ((Math.min(Math.max(pos.x, x - Math.floor(hs / 2)), (pos.x + pos.width - hs)) - pos.x) / (pos.width - hs)))));
      }
    }

    rdrag(x,y)
    {
      this.drag(x,y);
    }

    click(x, y)
    {
      if(this.drag)
      {
        this.drag(x,y);
      }
      if(this.drop)
      {
        this.drop(x,y);
      }
    }

    wheelup()
    {
      this.step(1);
    }

    wheeldown()
    {
      this.step(-1);
    }

    done()
    {
      UIElement.guiComponent.unfocus();
    }

    draw(pos)
    {
      if(this == UIElement.guiComponent.mousein ||
        this == UIElement.guiComponent.drag || this == UIElement.guiComponent.focus)
      {
        this.context.fillStyle = this.default;
      }
      else
      {
        this.context.fillStyle = this.bg;
      }
      this.rect(pos);

      if(this == UIElement.guiComponent.mousein ||
        this == UIElement.guiComponent.drag || this == UIElement.guiComponent.focus)
      {
        this.context.fillStyle = this.fg;
      }
      else
      {
        this.context.fillStyle = this.hilite;
      }

      var hs = this.hs;
      if(hs == 'auto')
      {
        if(this.axis == 'vertical')
        {
          var h = this.parent ? this.parent.height : pos.height;
          hs = Math.max(this.unit * 0.25, Math.min(pos.height, pos.height * h / (this.max - this.min + h)));
        }
        else
        {
          var w = this.parent ? this.parent.width : pos.width;
          hs = Math.max(this.unit * 0.25, Math.min(pos.width, pos.width * w / (this.max - this.min + w)));
        }
      }

      var handlepos = new Position();
      handlepos.x = this.axis == 'horizontal' ?
        Math.min(pos.x + pos.width - hs,
          Math.max(pos.x, pos.x + ((pos.width - hs) * ((this.current - this.min) / (this.max - this.min)))))
            : pos.x;

      handlepos.y = this.axis == 'vertical' ?
        Math.min(pos.y + pos.height - hs,
          Math.max(pos.y, pos.y + ((pos.height - hs) * ((this.current - this.min) / (this.max - this.min)))))
            : pos.y;

      handlepos.width = this.axis == 'horizontal' ? hs : this.width;
      handlepos.height = this.axis == 'vertical' ? hs : this.height;
      handlepos.radius = pos.radius;

      this.drawShape(handlepos);
      if(this.label)
      {
        this.context.textAlign = this.textAlign;
        this.context.textBaseline = this.textBaseline;
        this.context.fillStyle = this.labelfg;
        this.context.fillText(this.label,
          (this.axis == 'horizontal' ? pos.x - pos.width : pos.x + pos.width * 0.5),
          (this.axis == 'vertical' ? pos.y + pos.height * 0.5 : pos.y + pos.height * 0.5));
      }
    }
  }
  return ScrollElement;
});
