class UIElement
{
  constructor(label, pos, parent)
  {
    if(guiComponentInstance == null)
    {
      throw "GUI Component must be defined before creating an element.";
    }
    if(label instanceof GuiComponent)
    {
      throw "Passed GUI Component as first argument.";
    }
    this.guiComponent = guiComponentInstance;

    this._pos = new Position(pos);
    this._label = label;
    this._display = true;
    this._dt = 0;
    this._parent = parent;
    this._children = [];

    if(parent)
    {
      this._style = new Style(parent.style);
    }
    else
    {
      this._style = new Style(this.guiComponent.style);
    }
    this.guiComponent.add(this);
  }

  get x()
  {
    return this.pos.x;
  }

  set x(value)
  {
    this.pos.x = value;
  }

  get y()
  {
    return this.pos.y;
  }

  set y(value)
  {
    this.pos.y = value;
  }

  get width()
  {
    return this.pos.width;
  }

  set width(value)
  {
    this.pos.width = value;
  }

  get height()
  {
    return this.pos.height;
  }

  set height(value)
  {
    this.pos.height = value;
  }

  get radius()
  {
    return this.pos.radius;
  }

  set radius(value)
  {
    this.pos.radius = value;
  }

  get pos()
  {
    return this._pos;
  }

  get display()
  {
    return this._display;
  }

  set display(b)
  {
    this._display = b;
  }

  get bg()
  {
    return this.style.bg;
  }

  set bg(f)
  {
    this.style.bg = f;
  }

  get fg()
  {
    return this.style.fg;
  }

  set fg(f)
  {
    this.style.fg = f;
  }

  get label()
  {
    return this._label;
  }

  set label(b)
  {
    this._label = b;
  }

  get labelfg()
  {
    return this.style.labelfg;
  }

  set labelfg(f)
  {
    this.style.labelfg = f;
  }

  get hilite()
  {
    return this.style.hilite;
  }

  set hilite(f)
  {
    this.style.hilite = f;
  }

  get default()
  {
    return this.style.default;
  }

  set default(f)
  {
    this.style.default = f;
  }

  get dt()
  {
    return this._dt;
  }

  set dt(b)
  {
    this._dt = b;
  }

  get parent()
  {
    return this._parent;
  }

  get children()
  {
    return this._children;
  }

  get style()
  {
    return this._style;
  }

  get font()
  {
    return this.style.font;
  }

  set font(f)
  {
    this.style.font = f;
  }

  get unit()
  {
    return this.style.unit;
  }

  set unit(value)
  {
    this.style.unit = value;
  }

  set hs(f)
  {
    this.style.hs = f;
  }

  get hs()
  {
    return this.style.hs;
  }

  get textAlign()
  {
    return this.style.textAlign;
  }

  set textAlign(f)
  {
    this.style.textAlign = f;
  }

  get textBaseline()
  {
    return this.style.textBaseline;
  }

  set textBaseline(f)
  {
    this.style.textBaseline = f;
  }

  get mx()
  {
    return this.guiComponent.mx;
  }

  get my()
  {
    return this.guiComponent.my;
  }

  drawImage(pos)
  {
    context.drawImage(this.img, 0, 0, this.img.width, this.img.height, this.x, this.y, this.width, this.height);
  }

  drawShape(pos)
  {
    if(this.shape == "circle")
    {
      context.beginPath();
      context.arc(pos.x + pos.radius, pos.y + pos.radius, pos.radius, 0, Math.PI * 2);
      context.fill();
    }
    else
    {
      this.rect(pos);
    }
  }

  rect(pos, mode)
  {
    mode = mode || 'fill';
    if(mode == 'fill')
    {
      context.fillRect(pos.x, pos.y, pos.width, pos.height);
    }
    else
    {
      context.strokeRect(pos.x, pos.y, pos.width, pos.height);
    }
  }

  containsPoint(x,y)
  {
    var contains = true;
    var pos = this.getPosition();
    if(this.shape == 'circle')
    {
      var p = new Position(pos);
      p.x += this.radius;
      p.y += this.radius;
      if(!UIElement.withinRadius(x, y, p, this.scissor))
      {
        contains = false;
      }
    }
    else if(!UIElement.withinRect(x, y, pos, this.scissor))
    {
      contains = false;
    }
    return contains;
  }

  static withinRect(x, y, rect, scissor)
  {
    if(scissor)
    {
      return  rect.x <= x     && x <= rect.x + rect.width &&
              rect.y <= y     && y <= rect.y + rect.height &&
              scissor.x <= x  && x <= scissor.x + scissor.width &&
              scissor.y <= y  && y <= scissor.y + scissor.height;
    }
    return rect.x <= x && x <= rect.x + rect.width &&
          rect.y <= y && y <= rect.y + rect.height;
  }

  static getDistance(pos, target)
  {
    return Math.sqrt((pos.x - target.x) * (pos.x - target.x) + (pos.y - target.y) * (pos.y - target.y));
  }

  static withinRadius(x, y, circ, scissor)
  {
    if((x - circ.x) * (x - circ.x) + (y - circ.y) * (y - circ.y) < circ.radius * circ.radius)
    {
      if(scissor)
      {
        return scissor.x <= x && x <= scissor.x + scissor.width &&
              scissor.y <= y && y <= scissor.y + scissor.height;
      }
      else
      {
        return true;
      }
    }
    return false;
  }

  getParent()
  {
    if(this.parent)
    {
      return this.parent.getParent();
    }
    return this;
  }

  get maxW()
  {
    var maxw = 0;
    for(var i = 0; i < this.children.length; ++i)
    {
      var child = this.children[i];
      if(child != this.scrollv && child != this.scrollh && child.x + child.width > maxw)
      {
        maxw = child.x + child.width;
      }
    }
    return maxw;
  }

  get maxH()
  {
    var maxh = 0;
    for(var i = 0; i < this.children.length; ++i)
    {
      var child = this.children[i];
      if(child != this.scrollv && child != this.scrollh && child.y + child.height > maxh)
      {
        maxh = child.y + child.height;
      }
    }
    return maxh;
  }

  addChild(child, autostack)
  {
    if(!(child instanceof UIElement))
    {
      throw new TypeError("Must add UI element children");
    }

    if(autostack)
    {
      if(typeof autostack == "number" || autostack == "grid")
      {
        var limitx = typeof autostack == "number" ? autostack : this.width;
        var maxx = 0;
        var maxy = 0;
        for(var i = 0; i < this.children.length; ++i)
        {
          var element = this.children[i];
          if(element != this.scrollh && element != this.scrollv)
          {
            if(element.y > maxy)
            {
              maxy = element.y;
            }
            if(element.x + element.width + child.width <= limitx)
            {
              maxx = element.x + element.width;
            }
            else
            {
              maxx = 0;
              maxy = element.y + element.height;
            }
          }
        }
        child.x = maxx;
        child.y = maxy;
      }
      else if(autostack == "horizontal")
      {
        child.x = this.maxW;
      }
      else if(autostack == "vertical")
      {
        child.y = this.maxH;
      }
    }

    this.children.push(child);
    child._parent = this;
    child._style.append(this.style);
    if(this.scrollh)
    {
      this.scrollh.values.max = Math.max(this.maxW - this.width, 0);
    }
    if(this.scrollv)
    {
      this.scrollv.values.max = Math.max(this.maxH - this.height, 0);
    }
    return child;
  }

  static getIndex(list, val)
  {
    for(var i = 0; i < list.length; ++i)
    {
      if(list[i] == val)
      {
        return i;
      }
    }
  }

  remchild(child)
  {
    child._pos = child.getPosition();
    this.children.splice(UIElement.getIndex(this.children, child), 1);
    child._parent = null;
  }

  show()
  {
    this.display = true;
    for(var i = 0; i < this.children.length; ++i)
    {
      this.children[i].show();
    }
  }

  hide()
  {
    this.display = false;
    for(var i = 0; i < this.children.length; ++i)
    {
      this.children[i].hide();
    }
  }

  focus()
  {
    this.guiComponent.setfocus(this);
  }

  getPosition()
  {
    var pos = new Position(this.pos);
    if(this.parent)
    {
      var ppos = this.parent.getPosition();
      pos.x += ppos.x;
      pos.y += ppos.y;
      if(this.parent instanceof ScrollGroupElement && this != this.parent.scrollv && this != this.parent.scrollh)
      {
        if(this.parent.scrollv)
        {
          pos.y -= this.parent.scrollv.values.current;
        }
        if(this.parent.scrollh)
        {
          pos.x -= this.parent.scrollh.values.current;
        }
      }
    }
    return pos;
  }

  get scissor()
  {
    if(this.parent)
    {
      var scissor = this.parent.scissor;
      if(this.parent instanceof ScrollGroupElement && this != this.parent.scrollv && this != this.parent.scrollh)
      {
        scissor = new Position(this.parent.getPosition());
      }
      return scissor;
    }
  }

  set level(level)
  {
    if(level)
    {
      this.guiComponent.elements.splice(this.index, 1);
      this.guiComponent.elements.splice(level, 0, this);
      for(var i = 0; i < this.children.length; ++i)
      {
        var child = this.children[i];
        child.level = level + 1;
      }
    }
    else
    {
      this.guiComponent.elements.splice(this.index, 1);
      this.guiComponent.elements.push(this);
      for(var i = 0; i < this.children.length; ++i)
      {
        var child = this.children[i];
        child.level = null;
      }
    }
  }

  get level()
  {
    for(var i = 0; i < this.guiComponent.elements.length; ++i)
    {
      if(this.guiComponent.elements[i] == this)
      {
        return i;
      }
    }
  }

  get index()
  {
    return UIElement.getIndex(this.guiComponent.elements, this);
  }

  replace(replacement)
  {
    var newindex = this.index;
    this.guiComponent.rem(this);
    var oldindex = replacement.index;
    this.guiComponent.elements.splice(oldindex, 1);
    this.guiComponent.elements.splice(newindex, 0, replacement);
    return replacement;
  }
}