class ButtonElement extends UIElement
{
  draw(pos)
  {
    if(this.parent && this.value == this.parent.value)
    {
      if(this == this.guiComponent.mousein)
      {
        context.fillStyle = this.style.focus;
      }
      else
      {
        context.fillStyle = this.style.hilite;
      }
    }
    else
    {
      if(this == this.guiComponent.mousein)
      {
        context.fillStyle = this.style.hilite;
      }
      else
      {
        context.fillStyle = this.style.default;
      }
    }

    this.drawShape(pos);
    context.fillStyle = this.style.labelfg;

    if(this.shape == 'circle')
    {
      if(this.img)
      {
        this.drawImage(pos);
      }
      if(this.label)
      {
        context.textBaseline = "top";
        var y = this.img ? this.y + this.radius * 2 : this.y;
        context.fillText(this.label, this.x + this.radius, y, this.width);
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
        context.textBaseline = "top";
        var y = this.img ? this.y + this.height : this.y;
        context.fillText(this.label, this.x + this.radius, y, this.width);
      }
    }
  }
}
