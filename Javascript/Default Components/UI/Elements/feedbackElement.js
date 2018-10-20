define(['uiElement', 'game'], function(UIElement, Game)
{
  class FeedbackElement extends UIElement
  {
    constructor(label, pos, parent, autopos)
    {
      super(label, pos, parent);
      autopos = autopos == null ? true : autopos;
      if(autopos)
      {
        for(var i = 0; i < UIElement.guiComponent.elements.length; ++i)
        {
          var element = UIElement.guiComponent.elements[i];
          if(element != this && element instanceof FeedbackElement && element.autopos)
          {
            element.y += element.style.unit;
          }
        }
      }
      this.fg = "rgb(255,255,255)";
      this.alpha = 1;
      this.life = 5;
      this.autopos = autopos;
      this.fitWidth = false;
    }

    update(dt)
    {
      this.alpha -= dt / this.life;
      if(this.alpha < 0)
      {
        UIElement.guiComponent.rem(this);
        return;
      }

      this.fg = "rgba(255,255,255, " + this.alpha + ")";
    }

    draw(pos)
    {
      Game.context.fillStyle = this.fg;
      Game.context.textAlign = this.textAlign;
      Game.context.textBaseline = this.textBaseline;
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
  return FeedbackElement;
});
