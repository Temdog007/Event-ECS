define(['uiElement', 'scrollElement', 'game'], function(UIElement, ScrollElement, Game)
{
  class ScrollGroupElement extends UIElement
  {
    constructor(label, pos, parent, axis)
    {
      super(label, pos, parent);
      axis = axis || 'both';
      this.maxh = 0;
      if(axis != 'horizontal')
      {
        this.scrollv = new ScrollElement(null, null, this,
          {min : 0, max : 0, current : 0, step : this.style.unit, axis : 'vertical'});
        this.scrollv.x = this.width;
        this.scrollv.y = 0;
        this.scrollv.width = this.style.unit;
        this.scrollv.height = this.height;
      }
      if(axis != 'vertical')
      {
        this.scrollh = new ScrollElement(null, null, this,
          {min : 0, max : 0, current : 0, step : this.style.unit, axis : 'horizontal'});
        this.scrollh.x = 0;
        this.scrollh.y = this.height;
        this.scrollh.width = this.width;
        this.scrollh.height = this.style.unit;
      }
    }

    get width()
    {
      return super.width;
    }

    set width(f)
    {
      super.width = f;
      if(this.scrollv)
      {
        this.scrollv.x = this.width;
      }
      if(this.scrollh)
      {
        this.scrollh.width = this.width;
      }
    }

    get height()
    {
      return super.height;
    }

    set height(f)
    {
      super.height = f;
      if(this.scrollv)
      {
        this.scrollv.height = this.height;
      }
      if(this.scrollh)
      {
        this.scrollh.y = this.height;
      }
    }

    draw(pos)
    {
      this.context.fillStyle = this.bg;
      this.drawShape(pos);
      if(this.label)
      {
        this.context.textAlign = this.textAlign;
        this.context.textBaseline = this.textBaseline;
        this.context.fillStyle = this.labelfg;
        this.context.fillText(this.label, pos.x + pos.width * 0.5, pos.y, pos.width);
      }
    }
  }
  return ScrollGroupElement;
});
