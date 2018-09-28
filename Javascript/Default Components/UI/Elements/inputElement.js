class InputElement extends UIElement
{
  constructor(guiComponent, label, pos, parent, value, ispassword, passwordchar)
  {
    super(guiComponent, label, pos, parent);
    this.value = value ? value.toString() : '';
    this.cursor = this.value.length;
    this.textorigin = 0;
    this.cursorlife = 0;
    this.keyrepeat = true;
    this.ispassword = passwordchar ? passwordchar.toString() : '*';
  }

  update(dt)
  {
    if(this.cursor > this.value.length)
    {
      this.cursor = this.value.length;
    }
    if(this.guiComponent.focus == this)
    {
      if(this.cursorlife >= 1)
      {
        this.cursorlife = 0;
      }
      else
      {
        this.cursorlife += dt;
      }
    }
  }

  draw(pos)
  {
    if(this == this.guiComponent.focus)
    {
      context.fillStyle = this.style.bg;
    }
    else if(this == this.guiComponent.mousein)
    {
      context.fillStyle = this.style.hilite;
    }
    else
    {
      context.fillStyle = this.style.default;
    }

    this.drawShape(pos);

    context.textAlign = this.textAlign;
    context.textBaseline = this.textBaseline;
    
    var editw = this.width - 8;
    if(editw >= 1)
    {
      context.fillStyle = this.style.fg;
      var str = this.ispassword ? this.ispasswordchar.repeat(TextElement.utf8len(this.value.toString())) : this.value.toString();

      var cursorx = this.textorigin + pos.width;
      if(cursorx < 0)
      {
        this.textorigin = Math.min(0, this.textorigin - cursorx);
        cursorx = 0;
      }
      if(cursorx > editw - 1)
      {
        this.textorigin = Math.min(0, this.textorigin - cursorx + editw - 1);
        cursorx = editw - 1;
      }

      context.fillText(str, pos.x, pos.y + pos.height, pos.width);
      if(this == this.guiComponent.focus && this.cursorlife < 0.5)
      {
        context.fillRect(pos.x + cursorx, pos.y + 2, 1, pos.height - 2);
      }
    }
    if(this.label)
    {
      context.fillStyle = this.labelfg;
      context.fillText(this.label, pos.x, pos.y + pos.height - 8);
    }
  }

  click()
  {
    this.focus();
  }

  done()
  {
    this.guiComponent.unfocus();
  }

  keypress(key)
  {
    var save_life = this.cursorlife;
    this.cursorlife = 0;
    if(key == "backspace")
    {
      var cur = this.cursor;
      if(cur > 0)
      {
        this.cursor = TextElement.utf8char_begin(this.value, cur) - 1;
        this.value = this.value.substring(0, this.cursor) + this.value.substring(cur + 1);
      }
    }
    else if(key == "delete")
    {
      var cur = utf8char_after(this.value, this.cursor + 1);
      this.value = this.value.substring(0, this.cursor) + this.value.substring(cur);
    }
    else if(key == "left")
    {
      if(this.cursor > 0)
      {
        this.cursor = TextElement.utf8char_begin(this.value, this.cursor) - 1;
      }
    }
    else if(key == "right")
    {
      this.cursor = utf8char_after(this.value, this.cursor + 1) - 1;
    }
    else if (key == "home")
    {
      this.cursor = 0;
    }
    else if(key == 'end')
    {
      this.cursor = this.value.length;
    }
    else if(key == "tab" && this.next && this.next instanceof UIElement)
    {
      this.next.focus();
    }
    else if(key == "escape")
    {
      this.guiComponent.unfocus();
    }
    else
    {
      this.value = this.value.substring(0, this.cursor) + key + this.value.substring(this.cursor + 1);
    }
  }
}
