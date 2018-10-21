define(['uiElement', 'game'], function(UIElement, Game)
{
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
        this.context.fillStyle = this.labelfg;
        this.context.textAlign = this.textAlign;
        this.context.textBaseline = this.textBaseline;
        this.context.fillText(this.label, pos.x, pos.y + pos.height);
      }
    }
  }
  return ImageElement;
});
