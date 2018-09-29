class Style
{
  constructor(s)
  {
    this._unit = 16;
    this._font = "16px Arial";
    this._fg = "rgb(255,255,255)";
    this._bg = "rgb(64,64,64)";
    this._default = "rgb(96,96,96)";
    this._hilite = "rgb(128,128,128)";
    this._focus = "rgb(160,160,160)";
    this._textAlign = "left";
    this._textBaseline = "top";

    if(s)
    {
      this._unit = s.unit || this.unit;
      this._font = s.font || this.font;
      this._fg = s.fg || this.fg;
      this._bg = s.bg || this.bg;
      this._default = s.default || this.default;
      this._hilite = s.hilite || this.hilite;
      this.labelfg = s.labelfg;
      this._focus = s.focus || this.focus;
      this._textAlign = s.textAlign || this.textAlign;
      this._textBaseline = s.textBaseline || this.textBaseline;
    }
  }

  append(s)
  {
    this.unit = this.unit || s.unit;
    this.font = this.font || s.font;
    this.fg = this.fg || s.fg;
    this.bg = this.bg || s.bg;
    this.labelfg = s.labelfg;
    this.default = this.default || s.default;
    this.hilite = this.hilite || s.hilite;
    this.focus = this.focus || s.focus;
    this.textAlign = this.textAlign || s.textAlign;
    this.textBaseline = this.textBaseline || s.textBaseline;
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
    return this._hilite;
  }

  set hilite(d)
  {
    this._hilite = d;
  }

  get focus()
  {
    return this._focus;
  }

  set focus(d)
  {
    this._focus = d;
  }

  get hs()
  {
    return this._hs;
  }

  set hs(d)
  {
    this._hs = d;
  }

  get textAlign()
  {
    return this._textAlign;
  }

  set textAlign(f)
  {
    this._textAlign = f;
  }

  get textBaseline()
  {
    return this._textBaseline;
  }

  set textBaseline(f)
  {
    this._textBaseline = f;
  }
}
