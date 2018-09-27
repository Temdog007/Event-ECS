class GroupElement extends Element
{
  draw(pos)
  {
    context.fillStyle = this.style.bg;
    this.drawShape(pos);
    if(this.label)
    {
      context.fillStyle = this.style.labelfg;
      context.textBaseline = "top";
      context.fillText(this.label, this.x, this.y, this.width);
    }
  }
}
