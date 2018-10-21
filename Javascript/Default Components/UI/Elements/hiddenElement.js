define(['uiElement'], function(UIElement)
{
  class HiddenElement extends UIElement
  {
    draw(){}

    set dirty(v){}

    get dirty(){return false;}
  }
  return HiddenElement;
});
