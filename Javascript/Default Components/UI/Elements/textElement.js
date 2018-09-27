class TextElement extends Element
{
  draw(pos)
  {
    context.fillStyle = this.color;
    context.textBaseline = "top";
    context.fillText(this.label, this.x, this.y, this.width);
  }
}
