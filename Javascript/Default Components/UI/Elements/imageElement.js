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
        Game.context.fillStyle = this.labelfg;
        Game.context.textAlign = this.textAlign;
        Game.context.textBaseline = this.textBaseline;
        Game.context.fillText(this.label, pos.x, pos.y + pos.height);
      }
    }
  }
  return ImageElement;
});
