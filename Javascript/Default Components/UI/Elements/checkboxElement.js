define(['uiElement', 'game', 'position'],
function(UIElement, Game, Position)
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
        this.context.fillStyle = this.hilite;
      }
      else
      {
        this.context.fillStyle = this.default;
      }
      this.drawShape(pos);

      if(this.value)
      {
        this.context.fillStyle = this.fg;
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
        this.context.fillStyle = this.labelfg;
        this.context.textAlign = "center";
        this.context.textBaseline = "middle";
        var x, y, width;
        if(this.shape == "circle")
        {
          x = pos.x + pos.radius;
          y = pos.y + pos.radius;
          width = pos.w;
        }
        else
        {
          x = pos.x + pos.w * 0.5;
          y = pos.y + pos.h * 0.5;
          width = pos.w;
        }
        this.context.fillText(this.label, x, y, width);
      }
    }
  }
  return CheckboxElement;
});
