class TextElement extends UIElement
{
  draw(pos)
  {
    context.textBaseline = "top";
    context.fillText(this.label, pos.x, pos.y, this.width);
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
