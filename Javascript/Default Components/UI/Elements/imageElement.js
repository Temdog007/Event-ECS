class ImageElement extends Element
{
  constructor(guiComponent, label, pos, parent, img)
  {
    super(guiComponent, label, pos, parent);
    this.img = img;
  }

  draw(pos)
  {
    if(this.img)
    {
      this.drawImg(pos);
    }
  }
}
