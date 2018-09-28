class ScrollGroupElement extends UIElement
{
  constructor(guiComponent, label, pos, parent, axis)
  {
    super(guiComponent, label, pos, parent);
    axis = axis || 'both';
    this.maxh = 0;
    if(axis != 'horizontal')
    {
      this.scrollv = new ScrollElement(guiComponent, null, null, this,
        {min : 0, max : 0, current : 0, step : this.style.unit, axis : 'vertical'});
      this.scrollv.x = this.width;
      this.scrollv.y = 0;
      this.scrollv.width = this.style.unit;
      this.scrollv.height = this.height;
    }
    if(axis != 'vertical')
    {
      this.scrollh = new ScrollElement(guiComponent, null, null, this,
        {min : 0, max : 0, current : 0, step : this.style.unit, axis : 'horizontal'});
      this.scrollh.x = 0;
      this.scrollh.y = this.height;
      this.scrollh.width = this.width;
      this.scrollh.height = this.style.unit;
    }
  }

  draw(pos)
  {
    context.fillStyle = this.style.bg;
    this.drawShape(pos);
    if(this.label)
    {
      context.fillStyle = this.style.labelfg;
      context.fillText(this.label, pos.x, pos.y, pos.width);
    }
  }
}
