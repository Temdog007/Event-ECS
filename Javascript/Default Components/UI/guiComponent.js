define(['drawableComponent', 'style', 'position', 'game', 'systemlist'],
function(DrawableComponent, Style, Position, Game, Systems)
{
  var instanceObj = (function()
  {
    var guiComponentInstance;
    return {
      get obj()
      {
        return guiComponentInstance;
      },
      set obj(instance)
      {
        guiComponentInstance = instance;
      }
    };
  })();

  class GuiComponent extends DrawableComponent
  {
    constructor(entity)
    {
      super(entity);

      this._dblclickinterval = 0.25;
      this._mousedt = this.dblclickinterval;
      this._elements = [];

      this._mx = 0;
      this._my = 0;
      this.style = new Style();

      instanceObj.obj = this;
    }

    static get instance()
    {
      return instanceObj.obj;
    }

    get mx()
    {
      return this._mx;
    }

    get my()
    {
      return this._my;
    }

    get dblclickinterval()
    {
      return this._dblclickinterval;
    }

    get mousedt()
    {
      return this._mousedt;
    }

    get elements()
    {
      return this._elements;
    }

    add(element)
    {
      this.elements.push(element);
      if(element.parent)
      {
        element.parent.addChild(element);
      }
      return element;
    }

    rem(element)
    {
      if(element.parent)
      {
        element.parent.remchild(element);
      }
      while(element.children.length > 0)
      {
        for(var i = 0; i < element.children.length; ++i)
        {
          this.rem(element.children[i]);
        }
      }
      if(element == this.mousein){ this.mousein = null;}
      if(element == this.drag){this.drag = null;}
      if(element == this.focus) {this.unfocus();}
      var UIElement = require("uiElement");
      this.elements.splice(UIElement.getIndex(this.elements, element), 1);
    }

    setfocus(element)
    {
      if(element)
      {
        this.focus = element;
      }
    }

    unfocus()
    {
      this.focus = null;
    }

    eventUpdate(args)
    {
      this._mousedt += args.dt;
      var mousein = this.mousein;
      this.mousein = false;
      this.mouseover = false;
      if(this.drag)
      {
        this.handleDrag();
      }
      this.handleMouseIn();
      this.handleUpdate(args.dt);
      if(this.mousein != mousein)
      {
        if(this.mousein && this.mousein.enter)
        {
          this.mousein.enter();
        }
        if(mousein && mousein.leave)
        {
          mousein.leave();
        }
      }
    }

    handleDrag()
    {
      var element = this.drag;
      if(this.leftMouseDown)
      {
        if(typeof element.drag == "function")
        {
          element.drag(this.mx, this.my);
        }
        else
        {
          element.x = this.mx - element.offset.x;
          element.y = this.my - element.offset.y;
        }
      }
      else if(this.rightMouseDown)
      {
        if(typeof element.rdrag == "function")
        {
          element.rdrag(this.mx, this.my);
        }
        else
        {
          element.x = this.mx - element.offset.x;
          element.y = this.my - element.offset.y;
        }
      }
      for(var i = 0; i < this.elements.length; ++i)
      {
        if(this.elements[i] != element && this.elements[i].containsPoint(this.mx, this.my))
        {
          this.mouseover = this.elements[i];
        }
      }
    }

    handleMouseIn()
    {
      var ScrollGroupElement = require('scrollGroupElement');
      for(var i = this.elements.length - 1; i >= 0; --i)
      {
        var element = this.elements[i];
        if(element.display)
        {
          if(element.containsPoint(this.mx, this.my))
          {
            if(element.parent && element.parent instanceof ScrollGroupElement
              && element != element.parent.scrollv && element != element.parent.scrollh)
            {
              if(element.parent.containsPoint(this.mx, this.my))
              {
                this.mousein = element;
                break;
              }
            }
            else
            {
              this.mousein = element;
              break;
            }
          }
        }
      }
    }

    handleUpdate(dt)
    {
      for(var i = this.elements.length - 1; i >= 0; --i)
      {
        var element = this.elements[i];
        if(element.display && element.update)
        {
          if(element.updateInterval)
          {
            element.dt += dt;
            if(element.dt >= element.updateInterval)
            {
              element.dt = 0;
              element.update(dt);
            }
          }
          else
          {
            element.update(dt);
          }
        }
      }
    }

    doDraw(args)
    {
      for(var i = 0; i < this.elements.length; ++i)
      {
        var element = this.elements[i];
        if(element.display)
        {
          var pos = element.getPosition();
          var scissor = element.scissor;
          if(scissor)
          {
            Game.context.save();
            Game.context.beginPath();
            Game.context.rect(scissor.x, scissor.y, scissor.width, scissor.height);
            Game.context.clip();
          }
          Game.context.font = element.font;
          element.draw(pos);
          if(scissor)
          {
            Game.context.restore();
          }
        }
      }
      if(this.mousein && this.mousein.tip)
      {
        var element = this.mousein;
        var tippos = element.getPosition();
        tippos.x += this.style.unit * 0.5;
        tippos.y += this.style.unit * 0.5;
        pos.width = Game.context.measureText(element.tip).width + this.style.unit;

        Game.context.fillStyle = this.style.bg;
        var pos = new Position();
        pos.x = Math.max(0,
          Math.min(tippos.x, canvas.width - Game.context.measureText(element.tip).width + this.style.unit));
        pos.y = Math.max(0,
          Math.min(tippos.y, canvas.height - this.style.unit));
        pos.width = tippos.width;
        pos.height= this.style.unit;
        this.mousein.rect(pos);

        Game.context.fillStyle = this.style.fg;
        Game.context.textAlign = this.style.textAlign;
        Game.context.textBaseline = this.style.textBaseline;
        if(element.width > Game.context.measureText(element.tip).width)
        {
          Game.context.fillText(element.tip, pos.x, pos.y, element.width);
        }
        else
        {
          Game.context.fillText(element.tip, pos.x, pos.y);
        }
      }
    }

    eventMouseMoved(args)
    {
      this._mx = args.x;
      this._my = args.y;
      if(this.drag)
      {
        var element = this.drag;
        if(element.dragging)
        {
          var value = element.values.current;
          element.dragging(this.mouseover);
        }
      }
    }

    eventMouseDown(args)
    {
      this.unfocus();
      if(this.mousein)
      {
        var element = this.mousein;
        var HiddenElement = require('hiddenElement');
        if(!(element instanceof HiddenElement))
        {
          element.getParent().level = null;
        }
        if(args.buttonName == "left")
        {
          this.leftMouseDown = true;
          if(element.drag)
          {
            this.drag = element;
            var pos = element.getPosition();
            element.offset = {x : args.x - pos.x, y : args.y - pos.y};
          }
          if(this.mousedt < this.dblclickinterval && element.dblclick)
          {
            element.dblclick(args.x, args.y, args.buttonName);
          }
          else if(element.click)
          {
            element.click(args.x, args.y);
          }
        }
        else if(args.buttonName == "right" && element.rclick)
        {
          this.rightMouseDown = true;
          element.rclick(args.x, args.y);
        }
        else if(args.buttonName == "wu" && element.wheelup)
        {
          element.wheelup(args.x, args.y);
        }
        else if(args.buttonName == "wd" && element.wheeldown)
        {
          element.wheeldown(args.x, args.y);
        }
      }
      this._mousedt = 0;
    }

    eventMouseUp(args)
    {
      if(this.drag)
      {
        var element = this.drag;
        if(args.buttonName == "right")
        {
          this.rightMouseDown = false;
          if(element.rdrop)
          {
            element.rdrop(this.mouseover);
          }
          if(this.mouseover && this.mouseover.rcatch)
          {
            this.mouseover.rcatch(element);
          }
        }
        else
        {
          this.leftMouseDown = false;
          if(element.drop)
          {
            element.drop(this.mouseover);
          }
          if(this.mouseover && this.mouseover.catch)
          {
            this.mouseover.catch(element);
          }
        }
      }
      this.drag = null;
    }

    static handleWheel(element, dir, y)
    {
      if(!element.display){return false;}

      var ScrollGroupElement = require("scrollGroupElement");
      var ScrollElement = require("scrollElement");
      if(element instanceof ScrollGroupElement)
      {
        var scroll;
        if(dir == "horizontal")
        {
          scroll = element.scrollh;
        }
        else if(dir == "vertical")
        {
          scroll = element.scrollv;
        }

        if(scroll && scroll.values && scroll.values.min != scroll.values.max)
        {
          if(y < 0)
          {
            if(scroll.wheelup)
            {
              scroll.wheelup();
              return true;
            }
          }
          else
          {
            if(scroll.wheeldown)
            {
              scroll.wheeldown();
              return true;
            }
          }
        }
      }
      else if(element instanceof ScrollElement)
      {
        if(y < 0)
        {
          if(element.wheelup)
          {
            element.wheelup();
            return true;
          }
        }
        else
        {
          if(element.wheeldown)
          {
            element.wheeldown();
            return true;
          }
        }
      }

      for(var key in element.children)
      {
        if(GuiComponent.handleWheel(element.children[key], dir, y))
        {
          return true;
        }
      }

      return false;
    }

    eventMouseWheel(args)
    {
      var y = args.deltaY;
      if(y != 0)
      {
        for(var i = this.elements.length-1; i >= 0; --i)
        {
          var element = this.elements[i];
          if(element.display && GuiComponent.handleWheel(element, "vertical", y))
          {
            break;
          }
        }
      }
    }

    handleStick(dir, value)
    {
      if(Math.abs(value) > 0.25)
      {
        for(var i = this.elements.length-1; i >= 0; --i)
        {
          var element = this.elements[i];
          if(element.display && GuiComponent.handleWheel(element, dir, value))
          {
            break;
          }
        }
      }
    }

    eventGamepadAxis(args)
    {
      if(!args){return;}

      this.handleStick("horizontal", args.rx);
      this.handleStick("vertical", args.ry);
    }

    eventKeyDown(args)
    {
      if(this.focus)
      {
        if((args.key == "Enter") && this.focus.done)
        {
          this.focus.done();
        }
        if(this.focus && this.focus.keypress)
        {
          this.focus.keypress(args.key);
        }
      }
    }
  }

  return GuiComponent;
});
