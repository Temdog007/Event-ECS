class GroupElement extends UIElement
{
  draw(pos)
  {
    context.fillStyle = this.style.bg;
    this.drawShape(pos);
    if(this.label)
    {
      context.fillStyle = this.labelfg;
      context.textBaseline = this.textBaseline;
      context.textAlign = this.textAlign;
      if(this.fitWidth)
      {
        context.fillText(this.label, pos.x, pos.y, this.width);
      }
      else
      {
        context.fillText(this.label, pos.x, pos.y);
      }
    }
  }
}
