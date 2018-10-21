define(['uiElement', 'game'], function(UIElement, Game)
{
  class TextElement extends UIElement
  {
    constructor(label, pos, parent, autosize)
    {
      super(label, pos, parent);
      if(autosize)
      {
        this.context.font = this.font;
        this.width = this.context.measureText(this.label).width;
      }
    }

    wrapText(x, y, lineHeight)
    {
      lineHeight = lineHeight || 16;
      var words = this.label.split(' ');
      var line = '';

      var oldY = y;
      for(var n = 0; n < words.length; n++)
      {
        var testLine = line + words[n] + ' ';
        var metrics = this.context.measureText(testLine);
        var testWidth = metrics.width;
        if (testWidth > this.width && n > 0)
        {
          this.context.fillText(line, x, y);
          line = words[n] + ' ';
          y += lineHeight;
        }
        else
        {
          line = testLine;
        }
      }
      this.context.fillText(line, x, y);
      this.height = Math.max(lineHeight, Math.abs(y - oldY) + lineHeight);
    }

    draw(pos)
    {
      this.context.fillStyle = this.labelfg;
      this.context.textBaseline = this.textBaseline;
      this.context.textAlign = this.textAlign;

      if(this.fitWidth)
      {
        this.context.fillText(this.label, pos.x, pos.y, pos.width);
      }
      else if(this.wrap)
      {
        this.wrapText(pos.x, pos.y, this.lineHeight);
      }
      else
      {
        this.context.fillText(this.label, pos.x, pos.y);
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
  return TextElement;
});
