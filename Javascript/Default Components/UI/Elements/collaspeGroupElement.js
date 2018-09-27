class CollapseGroupElement extends Element
{
  constructor(guiComponent, label, pos, parent)
  {
    super(guiComponent, labe, pos, parent);
    this.view = true;
    this.orig = new Position(pos);
    
    this.control = new ButtonElement(guiComponent, '-', null, this);
    this.control.x = this.width - 16;
    this.control.click = this.parent.toggle;
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
