class CollapseGroupElement extends GroupElement
{
  constructor(guiComponent, label, pos, parent)
  {
    super(guiComponent, label, pos, parent);
    this.view = true;
    this.orig = new Position(pos);

    this.control = new ButtonElement(guiComponent, '-', null, this);
    this.control.click = function()
    {
      this.parent.toggle();
    }
  }

  set width(f)
  {
    super.width = f;
    this.control.x = this.width - this.control.width;
  }

  get width()
  {
    return super.width;
  }

  toggle()
  {
    this.view = !this.view;
    this.height = this.view ? this.orig.height : 16;
    for(var i = 0; i < this.children.length; ++i)
    {
      var child = this.children[i];
      if(child != this.control)
      {
        if(this.view)
        {
          child.show();
        }
        else
        {
          child.hide();
        }
      }
    }
    this.control.label = this.view ? '-' : '=';
  }
}
