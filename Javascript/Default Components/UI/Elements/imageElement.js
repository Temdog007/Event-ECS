class ImageElement extends UIElement
{
  constructor(label, pos, parent, img)
  {
    super(label, pos, parent);
    this.img = img;
  }

  draw(pos)
  {
    if(this.img)
    {
      this.drawImage(pos);
    }
    if(this.label)
    {
      context.fillStyle = this.labelfg;
      context.textAlign = this.textAlign;
      context.textBaseline = this.textBaseline;
      context.fillText(this.label, pos.x, pos.y + pos.height);
    }
  }
}
