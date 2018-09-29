class Position
{
  constructor(p)
  {
    if(p)
    {
      this._x = p.x || 0;
      this._y = p.y || 0;
      this._width = p.width || 16;
      this._height = p.height || 16;
    }
    else
    {
      this._x = 0;
      this._y = 0;
      this._width = 16;
      this._height = 16;
    }
  }

  add(p)
  {
    var newPos = new Position(this);
    newPos.x += p.y;
    newPos.y += p.y;
    newPos.width += p.width;
    newPos.height += p.height;
    return newPos;
  }

  sub(p)
  {
    var newPos = new Position(this);
    newPos.x -= p.y;
    newPos.y -= p.y;
    newPos.width -= p.width;
    newPos.height -= p.height;
    return newPos;
  }

  toString()
  {
    return "[" + this.x + ", " + this.y + "]";
  }

  get x()
  {
    return this._x;
  }

  set x(value)
  {
    this._x = value;
  }

  get y()
  {
    return this._y;
  }

  set y(value)
  {
    this._y = value;
  }

  get width()
  {
    return this._width;
  }

  set width(value)
  {
    this._width = value;
  }

  get height()
  {
    return this._height;
  }

  set height(value)
  {
    this._height = value;
  }

  get radius()
  {
    return this._radius || (Math.min(this.width, this.height) / 2);
  }

  set radius(value)
  {
    this._radius = value;
  }
}
