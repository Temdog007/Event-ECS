define(['groupElement', 'position', 'buttonElement'], function(GroupElement, Position, ButtonElement)
{
  class CollapseGroupElement extends GroupElement
  {
    constructor(label, pos, parent)
    {
      super(label, pos, parent);
      this.view = true;
      this.orig = new Position(pos);

      this.control = new ButtonElement('-', null, this);
      this.control.click = function()
      {
        this.parent.toggle();
      }
    }

    set view(v)
    {
      if(this._view == v){return;}

      this._view = v;
      this.dirty = true;
    }

    get view()
    {
      return this._view;
    }

    get dirty()
    {
      return super.dirty || this.orig.dirty;
    }

    set dirty(value)
    {
      super.dirty = value;
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
  return CollapseGroupElement;
});
