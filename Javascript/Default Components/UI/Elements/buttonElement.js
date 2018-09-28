class ButtonElement extends UIElement
{
  constructor(guiComponent, label, pos, parent, autosize)
  {
    super(guiComponent, label, pos, parent);
    if(autosize)
    {
      context.fillStyle = this.default;
      this.width = context.measureText(label).width;
    }
  }

  draw(pos)
  {
    if(this.parent && this.value == this.parent.value)
    {
      if(this == this.guiComponent.mousein)
      {
        context.fillStyle = this.style.focus;
      }
      else
      {
        context.fillStyle = this.hilite;
      }
    }
    else
    {
      if(this == this.guiComponent.mousein)
      {
        context.fillStyle = this.hilite;
      }
      else
      {
        context.fillStyle = this.default;
      }
    }
    this.drawShape(pos);

    context.textBaseline = this.textBaseline;
    context.textAlign = this.textAlign;
    context.fillStyle = this.labelfg;
    if(this.shape == 'circle')
    {
      if(this.img)
      {
        this.drawImage(pos);
      }
      if(this.label)
      {
        var y = this.img ? pos.y + this.radius * 2 : pos.y;
        context.fillText(this.label, pos.x, y, pos.radius * 2);
      }
    }
    else
    {
      if(this.img)
      {
        this.drawImage(pos);
      }
      if(this.label)
      {
        var y = this.img ? pos.y + this.height : pos.y;
        context.fillText(this.label, pos.x, y, pos.width);
      }
    }
  }
}
