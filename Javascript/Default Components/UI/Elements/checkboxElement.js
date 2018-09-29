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
      var tpos = new Position();
      tpos.x = pos.x + pos.width * 0.25;
      tpos.y = pos.y + pos.height * 0.25;
      tpos.width = pos.width * 0.5;
      tpos.height = pos.height * 0.5;
      tpos.radius = pos.radius * 0.5;
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
