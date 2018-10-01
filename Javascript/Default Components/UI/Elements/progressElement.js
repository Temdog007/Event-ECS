define(['uiElement', 'game'], function(UIElement, Game)
{
  class ProgressElement extends UIElement
  {
    constructor(label, pos, parent)
    {
      super(label, pos, parent);
      this.loaders = [];
      this.values = {min : 0, max : 0, current : 0, step : 1, axis : 'vertical'};
    }

    update(dt)
    {
      for(var i = 0; i < this.loaders.length; ++i)
      {
        var loader = this.loaders[i];
        if(loader.status == 'waiting')
        {
          try
          {
            loader.result = loader.func();
            loader.status = 'done';
            ++this.values.current;
          }
          catch (e)
          {
            loader.result = e;
            loaders.status = 'error';
          }
          break;
        }
        if(i == this.loaders.length - 1)
        {
          this.done();
        }
      }
    }

    draw(pos)
    {
      Game.context.fillStyle = this.default;
      this.drawShape(pos);
      Game.context.fillStyle = this.fg;
      this.rect({x : pos.x, y : pos.y, width : pos.width * (this.values.current / this.values.max), height : pos.height});
      if(this.label)
      {
        Game.context.fillStyle = this.labelfg;
        Game.context.textAlign = this.textAlign;
        Game.context.textBaseline = this.textBaseline;
        if(this.fitWidth)
        {
          Game.context.fillText(this.label, pos.x, pos.y, pos.width);
        }
        else
        {
          Game.context.fillText(this.label, pos.x, pos.y);
        }
      }
    }

    done()
    {
      UIElement.guiComponent.rem(this);
    }

    add(loader)
    {
      this.loaders.push({status : 'waiting', func : loader});
      ++this.values.max;
    }
  }
  return ProgressElement;
});
