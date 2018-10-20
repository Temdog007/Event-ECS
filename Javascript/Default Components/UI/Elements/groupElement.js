define(['uiElement', 'game'], function(UIElement, Game)
{
  class GroupElement extends UIElement
  {
    draw(pos)
    {
      Game.context.fillStyle = this.style.bg;
      this.drawShape(pos);
      if(this.label)
      {
        Game.context.fillStyle = this.labelfg;
        Game.context.textBaseline = this.textBaseline;
        Game.context.textAlign = this.textAlign;

        if(this.fitWidth)
        {
          Game.context.fillText(this.label, pos.x, pos.y, pos.width);
        }
        else
        {
          Game.context.fillText(this.label, pos.x, pos.y);
        }
      }
    }
  }
  return GroupElement;
});
