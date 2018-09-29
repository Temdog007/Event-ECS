class InputElement extends UIElement
{
  constructor(label, pos, parent, value, ispassword, passwordchar)
  {
    super(label, pos, parent);
    this.value = value ? value.toString() : '';
    this.cursor = this.value.length;
    this.textorigin = 0;
    this.cursorlife = 0;
    this.ispassword = ispassword || false;
    this.ispasswordchar = passwordchar ? passwordchar.toString() : '*';
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
      context.fillStyle = this.bg;
    }
    else if(this == this.guiComponent.mousein)
    {
      context.fillStyle = this.hilite;
    }
    else
    {
      context.fillStyle = this.default;
    }
    this.drawShape(pos);

    context.textAlign = this.textAlign;
    context.textBaseline = this.textBaseline;

    var editw = pos.width - this.unit * 0.5;
    if(editw >= 1)
    {
      context.save();

      context.beginPath();
      context.rect(pos.x + this.style.unit * 0.25, pos.y, editw, pos.height);
      context.clip();

      context.fillStyle = this.fg;
      var str = this.ispassword ? this.ispasswordchar.repeat(TextElement.utf8len(this.value.toString())) : this.value.toString();

      var cursorx = this.textorigin + context.measureText(str.substring(0, this.cursor)).width;
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

      if(this.fitWidth)
      {
        context.fillText(str, pos.x + this.unit * 0.25 + this.textorigin, pos.y + (pos.height - this.unit) * 0.5, pos.width);
      }
      else
      {
        context.fillText(str, pos.x + this.unit * 0.25 + this.textorigin, pos.y + (pos.height - this.unit) * 0.5);
      }

      if(this == this.guiComponent.focus && this.cursorlife < 0.5)
      {
        context.fillRect(pos.x + this.style.unit * 0.25 + cursorx, pos.y + this.style.unit * 0.125,
              1, pos.height - this.style.unit * 0.25);
      }

      context.restore();
    }
    if(this.label)
    {
      context.fillStyle = this.labelfg;
      context.fillText(this.label, pos.x - ((this.style.unit * 0.5) + context.measureText(this.label).width),
        pos.y + ((this.height - this.style.unit) * 0.5));
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
    if(key == "Backspace")
    {
      var cur = this.cursor;
      if(cur > 0)
      {
        this.cursor = TextElement.utf8char_begin(this.value, cur) - 1;
        this.value = this.value.substring(0, this.cursor) + this.value.substring(cur + 1);
      }
    }
    else if(key == "Delete")
    {
      var cur = TextElement.utf8char_after(this.value, this.cursor + 1);
      this.value = this.value.substring(0, this.cursor) + this.value.substring(cur);
    }
    else if(key == "ArrowLeft")
    {
      if(this.cursor > 0)
      {
        this.cursor = TextElement.utf8char_begin(this.value, this.cursor) - 1;
      }
    }
    else if(key == "ArrowRight")
    {
      this.cursor = TextElement.utf8char_after(this.value, this.cursor + 1) - 1;
    }
    else if (key == "Home")
    {
      this.cursor = 0;
    }
    else if(key == 'End')
    {
      this.cursor = this.value.length;
    }
    else if(key == "Tab" && this.next && this.next instanceof UIElement)
    {
      this.next.focus();
    }
    else if(key == "Escape")
    {
      this.guiComponent.unfocus();
    }
    else if(key != "Shift" && key != "CapsLock" && key != "Tab")
    {
      this.value = this.value.substring(0, this.cursor) + key + this.value.substring(this.cursor + 1);
      this.cursor += key.length;
    }
  }
}
