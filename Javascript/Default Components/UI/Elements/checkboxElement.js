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
      context.fillStyle = this.hilite;
    }
    else
    {
      context.fillStyle = this.default;
    }
    this.drawShape(pos);

    if(this.value)
    {
      context.fillStyle = this.fg;
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
      context.fillStyle = this.labelfg;
      this.textAlign = this.textAlign;
      this.textBaseline = this.textBaseline;
      context.fillText(this.label, pos.x, pos.y, pos.radius * 2);
    }
  }
}
