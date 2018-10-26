define(['drawableComponent', 'game'],
function(Component, Game)
{
  class BodyDrawerComponent extends Component
  {
    constructor(entity)
    {
      super(entity);
      this.setDefaults({
        staticColor : "lime",
        dynamicColor : "pink",
        fill : true
      });
    }

    get body()
    {
      return this.get("body");
    }

    get color()
    {
      var body = this.body;
      if(body.isStatic)
      {
        return this.get("staticColor");
      }
      else
      {
        return this.get("dynamicColor");
      }
    }

    get alpha()
    {
      return this.body.isSleeping ? 0.5 : 1;
    }

    eventDraw()
    {
      this.context.save();

      this.context.globalAlpha = this.alpha;
      this.context.fillStyle = this.color;
      this.context.strokeStyle = this.color;

      var body = this.body;
      if(body)
      {
        var verts = body.vertices;
        this.context.beginPath();
        this.context.moveTo(verts[0].x, verts[0].y);
        for(var i = 1; i < verts.length; ++i)
        {
          this.context.lineTo(verts[i].x, verts[i].y);
        }
        this.context.closePath();
        if(this.data.fill)
        {
          this.context.fill();
        }
        else
        {
          this.context.stroke();
        }
      }

      this.context.restore();
    }
  }

  return BodyDrawerComponent;
});
