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
        this.mousein.leave();
      }
    }
  }

  handleDrag()
  {
    var element = this.drag;
    if(this.mouseDown == "left")
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
    else if(this.mouseDown == "right")
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
          context.save();
          context.beginPath();
          context.rect(scissor.x, scissor.y, scissor.width, scissor.height);
          context.stroke();
          context.clip();
        }
        context.font = element.font;
        element.draw(pos);
        if(scissor)
        {
          context.restore();
        }
      }
    }
  }
}

if(path == null)
{
  var path = "";
}

function loadElement(elementSource, func)
{
  var s = document.createElement("script");
  s.src = path+"Elements/"+elementSource+".js";
  s.onload = func;
  document.body.appendChild(s);
}

function loadElements(elements, onelementsloaded)
{
  var scriptsLoaded = 0;
  function scriptLoaded()
  {
    if(++scriptsLoaded == elements.length)
    {
      onelementsloaded();
    }
  }

  for(var index in elements)
  {
    loadElement(elements[index], scriptLoaded);
  }
}

loadElements(["../position", "../style", "uielement"], function()
{
  window.dispatchEvent(new Event('uielementloaded'));
});

window.addEventListener("uielementloaded", function()
{
  loadElements(["buttonElement", "checkboxElement", "collapseGroupElement",
    "feedbackElement", "groupElement", "hiddenElement", "imageElement",
    "inputElement", "optionElement", "progressElement", "scrollElement",
     "textElement", "typeTextElement"],
    function()
     {
       window.dispatchEvent(new Event('guiLoaded'));
     });
});
