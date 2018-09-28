class CheckboxElement extends UIElement
{
  constructor(guiComponent, label, pos, parent, value)
  {
    super(guiComponent, label, pos, parent);
    this.value = value;
  }

  click()
  {
    this.value = !this.value;
  }

  draw(pos)
  {
    if(this == this.guiComponent.mousein)
    {
      context.fillStyle = this.style.hilite;
    }
    else
    {
      context.fillStyle = this.style.default;
    }

    this.drawShape(pos);

    if(this.value)
    {
      context.fillStyle = this.style.fg;
      var tpos = new Position(pos);
      tpos.x += tpos.width * 0.25;
      tpos.y += tpos.height * 0.25;
      tpos.width *= 0.5;
      tpos.height *= 0.5;
      tpos.radius = tpos.radius * 0.5;
      this.drawShape(tpos);
    }
    if(this.label)
    {
      context.fillStyle = this.style.labelfg;
      context.fillText(this.label, this.x, this.y, this.radius * 2);
    }
  }
}
