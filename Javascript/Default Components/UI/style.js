class Style
{
  constructor(s)
  {
    if(s)
    {
      this._unit = s.unit;
      this._font = s.font;
      this._fg = s.fg;
      this._bg = s.bg;
      this._default = s.default;
      this._hilite = s.hilite;
      this._focus = s.focus;
    }
    else
    {
      this._unit = 16;
      this._font = "16px Arial";
      this._fg = "white";
      this._bg = "rgb(0.25,0.25,0.25)";
      this._default = "rgb(0.375,0.375;0.375)";
      this._hilite = "rgb(0.5,0.5,0.5)";
      this._focus = "rgb(0.6274509803921569,0.6274509803921569,0.6274509803921569)";
    }
  }

  get unit()
  {
    return this._unit;
  }

  set unit(u)
  {
    this._unit = u;
  }

  get font()
  {
    return this._font;
  }

  set font(f)
  {
    this._font = f;
  }

  get fg()
  {
    return this._fg;
  }

  set fg(f)
  {
    this._fg = f;
  }

  get bg()
  {
    return this._bg;
  }

  set bg(f)
  {
    this._bg = f;
  }

  get labelfg()
  {
    return this._labelfg || this.fg;
  }

  set labelfg(f)
  {
    this._labelfg = f;
  }

  get default()
  {
    return this._default;
  }

  set default(d)
  {
    this._default = d;
  }

  get hilite()
  {
    this._hilite;
  }

  set hitlite(d)
  {
    this._hilite = d;
  }

  get focus()
  {
    this._focus;
  }

  set focus(d)
  {
    this._focus = d;
  }

  get hs()
  {
    this._hs;
  }

  set hs(d)
  {
    this._hs = d;
  }
}
