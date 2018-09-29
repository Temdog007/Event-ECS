class TextElement extends UIElement
{
  constructor(guiComponent, label, pos, parent, autosize)
  {
    super(guiComponent, label, pos, parent);
    if(autosize)
    {
      context.font = this.font;
      this.width = context.measureText(this.label).width;
      console.log(this.label, this.width);
    }
  }

  draw(pos)
  {
    context.fillStyle = this.labelfg;
    context.textBaseline = this.textBaseline;
    context.textAlign = this.textAlign;
    if(this.fitWidth)
    {
      context.fillText(this.label, pos.x, pos.y, pos.width);
    }
    else
    {
      context.fillText(this.label, pos.x, pos.y);
    }

  }

  static utf8char_begin(s, idx)
  {
    var b = s.charCodeAt(idx);
    while(b && b >= 0x80 && b < 0xC0)
    {
      b = s.charCodeAt(--idx);
    }
    return idx;
  }

  static utf8char_after(s, idx)
  {
    if(idx <= s.length)
    {
      ++idx;
      var b = s.charCodeAt(idx);
      while(b && b >= 0x80 && b < 0xC0)
      {
        b = s.charCodeAt(++idx);
      }
    }

    return idx;
  }

  static utf8len(s)
  {
    var p = 0;
    for(var i = 0; i < s.length; ++i)
    {
      var c = s.charCodeAt(i);
      if(c >= 0x80 && x < 0xC0)
      {
        ++p;
      }
    }
    return p;
  }
}
