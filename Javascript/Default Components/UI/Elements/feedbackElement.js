class FeedbackElement extends Element
{
  constructor(guiComponent, label, pos, parent, autopos)
  {
    super(guiComponent, label, pos, parent);
    autopos = autopos == null ? true : autopos;
    if(autopos)
    {
      for(var i = 0; i < guiComponent.elements.length; ++i)
      {
        var element = this.elements[i];
        if(element != this && element instancoef FeedbackElement && element.autopos)
        {
          element.y += element.style.unit;
        }
      }
    }
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

    this.style.fg = "rgba(1,1,1," + this.alpha +")"
  }

  draw(pos)
  {
    context.fillStyle = this.style.fg;
    context.fillText(this.label, pos.x, pos.y, pos.width);
  }
}
