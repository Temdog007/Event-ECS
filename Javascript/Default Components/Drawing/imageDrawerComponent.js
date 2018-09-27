class ImageDrawerComponent extends DrawableComponent
{
  constructor(entity)
  {
    super(entity);
    this.setDefaults({
      x : 0,
      y : 0,
      width : 100,
      height : 100
    });
  }

  doDraw(args)
  {
    var d = this.get("drawable");
    if(d == null) {return;}

    var data = this.data;
    context.drawImage(d,
      0, 0, d.width, d.height,
      data.x, data.y, data.width, data.height);
  }
}
