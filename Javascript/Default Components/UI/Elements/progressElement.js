class ProgressElement extends Element
{
  constructor(guiComponent, label, pos, parent)
  {
    super(guiComponent, label, pos, parent);
    this.loaders = [];
    this.values = {min : 0, max : 0, current : 0, step : 1, axis : 'vertical'};
  }

  update(dt)
  {
    for(var i = 0; i < this.loaders.length; ++i)
    {
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
      if(i == this.loaders.length -1)
      {
        this.done();
      }
    }
  }

  draw(pos)
  {
    context.fillStyle = this.style.default;
    this.drawShape(pos);
    context.fillStyle = this.style.fg;
    this.rect({x : pos.x, y : pos.y, : width : pos.width * (this.values.current / this.values.max), h = pos.h});
    if(this.label)
    {
      context.fillStyle = this.style.labelfg;
      context.fillText(this.label, pos.x, pos.y, pos.width);
    }
  }

  done()
  {
    this.guiComponent.rem(this);
  }

  add(loader)
  {
    this.loaders.push({status : 'waiting', func : loader});
    ++this.values.max;
  }
}
