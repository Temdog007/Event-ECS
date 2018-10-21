define(['uiElement', 'game'], function(UIElement, Game)
{
  class ButtonElement extends UIElement
  {
    constructor(label, pos, parent, autosize)
    {
      super(label, pos, parent);
      if(autosize)
      {
        this.context.fillStyle = this.default;
        this.width = this.context.measureText(label).width;
      }
    }

    draw(pos)
    {
      if(this.parent && this.value == this.parent.value)
      {
        if(this == UIElement.guiComponent.mousein)
        {
          this.context.fillStyle = this.style.focus;
        }
        else
        {
          this.context.fillStyle = this.hilite;
        }
      }
      else
      {
        if(this == UIElement.guiComponent.mousein)
        {
          this.context.fillStyle = this.hilite;
        }
        else
        {
          this.context.fillStyle = this.default;
        }
      }
      this.drawShape(pos);

      this.context.textBaseline = "middle";
      this.context.textAlign = "center";
      this.context.fillStyle = this.labelfg;
      if(this.shape == 'circle')
      {
        if(this.img)
        {
          this.drawImage(pos);
        }
        if(this.label)
        {
          var y = this.img ? pos.y + this.radius * 2 : pos.y;
          this.context.fillText(this.label, pos.x + pos.width * 0.5, y + pos.height * 0.5, pos.radius * 2);
        }
      }
      else
      {
        if(this.img)
        {
          this.drawImage(pos);
        }
        if(this.label)
        {
          var y = this.img ? pos.y + this.height : pos.y;
          this.context.fillText(this.label, pos.x + pos.width * 0.5, y + pos.height * 0.5, pos.width);
        }
      }
    }
  }
  return ButtonElement;
});
