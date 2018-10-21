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

    get alpha()
    {
      return this._alpha;
    }

    set alpha(value)
    {
      if(value == this._alpha){return;}

      this._alpha = value;
      this.dirty = true;
    }

    get life()
    {
      return this._alpha;
    }

    set life(value)
    {
      if(value == this._life){return;}

      this._life = value;
      this.dirty = true;
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
      this.context.fillStyle = this.fg;
      this.context.textAlign = this.textAlign;
      this.context.textBaseline = this.textBaseline;
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
  return FeedbackElement;
});
