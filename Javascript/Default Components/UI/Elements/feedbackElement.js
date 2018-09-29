class FeedbackElement extends UIElement
{
  constructor(label, pos, parent, autopos)
  {
    super(label, pos, parent);
    autopos = autopos == null ? true : autopos;
    if(autopos)
    {
      for(var i = 0; i < this.guiComponent.elements.length; ++i)
      {
        var element = this.guiComponent.elements[i];
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
  }

  update(dt)
  {
    this.alpha -= dt / this.life;
    if(this.alpha < 0)
    {
      this.guiComponent.rem(this);
      return;
    }

    this.fg = "rgba(255,255,255, " + this.alpha + ")";
  }

  draw(pos)
  {
    context.fillStyle = this.fg;
    context.textAlign = this.textAlign;
    context.textBaseline = this.textBaseline;
    if(this.fitWidth)
    {
      context.fillText(this.label, pos.x, pos.y, pos.width);
    }
    else
    {
      context.fillText(this.label, pos.x, pos.y);
    }
  }
}
