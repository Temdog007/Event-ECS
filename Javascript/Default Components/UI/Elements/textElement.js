define(['uiElement', 'game'], function(UIElement, Game)
{
  class TextElement extends UIElement
  {
    constructor(label, pos, parent, autosize)
    {
      super(label, pos, parent);
      if(autosize)
      {
        Game.context.font = this.font;
        this.width = Game.context.measureText(this.label).width;
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
        var metrics = Game.context.measureText(testLine);
        var testWidth = metrics.width;
        if (testWidth > this.width && n > 0)
        {
          Game.context.fillText(line, x, y);
          line = words[n] + ' ';
          y += lineHeight;
        }
        else
        {
          line = testLine;
        }
      }
      Game.context.fillText(line, x, y);
      this.height = Math.max(lineHeight, Math.abs(y - oldY) + lineHeight);
    }

    draw(pos)
    {
      Game.context.fillStyle = this.labelfg;
      Game.context.textBaseline = this.textBaseline;
      Game.context.textAlign = this.textAlign;

      if(this.fitWidth)
      {
        Game.context.fillText(this.label, pos.x, pos.y, pos.width);
      }
      else if(this.wrap)
      {
        this.wrapText(pos.x, pos.y, this.lineHeight);
      }
      else
      {
        Game.context.fillText(this.label, pos.x, pos.y);
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
