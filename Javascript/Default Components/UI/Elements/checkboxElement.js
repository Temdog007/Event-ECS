define(['uiElement', 'game', 'position'], function(UIElement, Game, Position)
{
  class CheckboxElement extends UIElement
  {
    constructor(label, pos, parent, value)
    {
      super(label, pos, parent);
      this.value = value;
    }

    click()
    {
      this.value = !this.value;
    }

    draw(pos)
    {
      if(this == UIElement.guiComponent.mousein)
      {
        Game.context.fillStyle = this.hilite;
      }
      else
      {
        Game.context.fillStyle = this.default;
      }
      this.drawShape(pos);

      if(this.value)
      {
        Game.context.fillStyle = this.fg;
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
        Game.context.fillStyle = this.labelfg;
        this.textAlign = this.textAlign;
        this.textBaseline = this.textBaseline;
        Game.context.fillText(this.label, pos.x, pos.y, pos.radius * 2);
      }
    }
  }
  return CheckboxElement;
});
