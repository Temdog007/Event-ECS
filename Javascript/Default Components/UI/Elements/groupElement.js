define(['uiElement', 'game'], function(UIElement, Game)
{
  class GroupElement extends UIElement
  {
    draw(pos)
    {
      this.context.fillStyle = this.style.bg;
      this.drawShape(pos);
      if(this.label)
      {
        this.context.fillStyle = this.labelfg;
        this.context.textBaseline = this.textBaseline;
        this.context.textAlign = this.textAlign;

        if(this.fitWidth)
        {
          this.context.fillText(this.label, pos.x, pos.y, pos.width);
        }
        else
        {
          this.context.fillText(this.label, pos.x, pos.y);
        }
      }
    }
  }
  return GroupElement;
});
