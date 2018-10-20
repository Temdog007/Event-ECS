define(function()
{
  class Position
  {
    constructor(p)
    {
      if(p)
      {
        this._x = p.x || 0;
        this._y = p.y || 0;
        if(p instanceof Position ? p._radius : (p.radius || p.r))
        {
          this._radius = p._radius || p.r;
          this.width = this.radius * 2;
          this.height = this.radius * 2;
        }
        else
        {
          this._width = p.width || p.w || 16;
          this._height = p.height || p.h || 16;
        }
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
      newPos.radius += p.radius;
      return newPos;
    }

    sub(p)
    {
      var newPos = new Position(this);
      newPos.x -= p.y;
      newPos.y -= p.y;
      newPos.width -= p.width;
      newPos.height -= p.height;
      newPos.radius -= p.radius;
      return newPos;
    }

    combine(p)
    {
      var newPos = new Position();
      newPos.x = Math.min(this.x, p.x);
      newPos.y = Math.min(this.y, p.y);
      newPos.r = Math.min(this.r, p.r);
      newPos.w = Math.min(this.w, p.w);
      newPos.h = Math.min(this.h, p.h);
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

    get w()
    {
      return this.width;
    }

    set w(value)
    {
      this.width = value;
    }

    get height()
    {
      return this._height;
    }

    set height(value)
    {
      this._height = value;
    }

    get h()
    {
      return this.height;
    }

    set h(value)
    {
      this.height = value;
    }

    get radius()
    {
      return this._radius || (Math.min(this.width, this.height) / 2);
    }

    set radius(value)
    {
      this._radius = value;
    }

    get r()
    {
      return this.radius;
    }

    set r(value)
    {
      this.radius = value;
    }
  }

  return Position;
});
