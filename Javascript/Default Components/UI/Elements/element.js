class Element
{
  constructor(guiComponent, label, pos, parent)
  {
    this.guiComponent = guiComponent;
    guiComponent:addChild(this);

    this._pos = new Position(pos);
    this._label = label;
    this._display = true;
    this._dt = 0;
    this._parent = parent;
    this._children = [];
    this._style = new Style();
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

  get label()
  {
    return this._label;
  }

  set label(b)
  {
    this._label = b;
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

  drawImage(pos)
  {
    context.drawImage(this.img, 0, 0, this.img.width, this.img.height, this.x, this.y, this.width, this.height);
  }

  drawShape(pos)
  {
    if(this.shape == "circle")
    {
      context.beginPath();
      context.arc(pos.x + pos.radius, pos.y + pos.radius, 0, Math.PI * 2);
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
      if(!this.withinRadius(x, y, p, this.scissor))
      {
        contains = false;
      }
    }
    else if(!this.withinRect(x, y, pos, this.scissor))
    {
      contains = false;
    }
    return contains;
  }

  static withinRect(x, y, rect, scissor)
  {
    if(scissor)
    {
      return
        rect.x <= x     && x <= rect.x + rect.width &&
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

  static withinRadius(pos, circ, scissor)
  {
    if((pos.x - circ.x) * (pos.x - circ.x) + (pos.y - circ.y) * (pos.y - circ.y) < circ.radius * circ.radius)
    {
      if(scissor)
      {
        return scissor.x <= pos.x && pos.x <= scissor.x + scissor.width &&
              scissor.y <= pos.y && pos.y <= scissor.y + scissor.height;
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

  getMaxW()
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

  getMaxH()
  {
    var maxh = 0;
    for(var i = 0; i < this.children.length; ++i)
    {
      var child = this.children[i];
      if(child != this.scrollv && child != this.scrollh && child.y + child.height > maxh)
      {
        maxh = child.x + child.width;
      }
    }
    return maxh;
  }

  addChild(child, autostack)
  {
    if(autostack)
    {
      if(typeof autostack == "number" || typeof autostack == "grid")
      {
        var limitx = typeof autostack == "number" ? autostack : this.width;
        var maxx = 0;
        var maxy = 0;
        for(var i = 0; i < this.children.length; ++i)
        {
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
      }
      else if(autostack == "horizontal")
      {
        chid.pos.x = this.getMaxW();
      }
      else if(autostack == "vertical")
      {
        child.pos.y = this.getMaxH();
      }
    }

    this.children.push(child);
    child.parent = this;
    child.style = new Style(this.style);
    if(this.scrollh)
    {
      this.scrollh.values.max = Math.max(this.getMaxW() - this.width, 0);
    }
    if(this.scrollv)
    {
      this.scrollv.values.max = Math.max(this.getMaxH() - this.height, 0);
    }
    return child;
  }

  remchild(child)
  {
    child.pos = child.getPosition();
    this.children.splice(this.getIndex(child), 1);
    child.parent = null;
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
    this.guiComponent.setFocus(this);
  }

  getPosition()
  {
    var pos = new Position(this.pos);
    if(this.parent)
    {
      var ppos = 0;
      ppos = this.parent.getPosition();
      pos.x += ppos.x;
      pos.y += ppos.y;
      if(this.parent instanceof ScrollGroupElement && this != this.parent.scrollv && this != this.parent.scrollh)
      {
        if(this.parent.scrollv)
        {
          pos.y -= this.parent.scrollv.values.current;
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
}