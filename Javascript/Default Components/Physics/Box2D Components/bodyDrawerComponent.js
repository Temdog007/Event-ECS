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
        kinematicColor : "blue",
        dynamicColor : "pink"
      });
    }

    get body()
    {
      return this.get("body");
    }

    get color()
    {
      var body = this.body;
      var type = body.GetType();
      if(type == b2Body.b2_dynamicBody)
      {
        return this.get("dynamicColor");
      }
      else if(type == b2Body.b2_kinematicBody)
      {
        return this.get("kinematicColor");
      }
      else
      {
        return this.get("staticColor");
      }
    }

    get alpha()
    {
      var body = this.body;
      return (body.IsActive() && body.IsAwake()) ? 1 : 0.5;
    }

    eventDraw()
    {
      this.context.save();

      this.context.globalAlpha = this.alpha;
      this.context.strokeStyle = this.color;

      var body = this.body;
      if(body)
      {
        var current = body.GetFixtureList();
        while(current)
        {
          var shapeType = current.GetType();
          if(shapeType == b2Shape.e_circleShape)
          {
            var shape = current.GetShape();
            var center = shape.m_p;
            center = body.GetWorldPoint(center);
            this.context.beginPath();
            this.context.arc(
              center.x*Game.mpp,
              center.y*Game.mpp,
              shape.m_radius*Game.mpp,
              0, Math.PI * 2);
            this.context.stroke();
          }
          else if(shapeType == b2Shape.e_polygonShape)
          {
            var shape = current.GetShape();
            var vertices = shape.GetVertices();
            this.context.beginPath();
            for(var i in vertices)
            {
              var vertex = body.GetWorldPoint(vertices[i]);
              var x = vertex.x, y = vertex.y;
              x *= Game.mpp;
              y *= Game.mpp;
              if(i == 0)
              {
                this.context.moveTo(x,y);
              }
              else
              {
                this.context.lineTo(x,y);
              }
            }
            this.context.closePath();
            this.context.stroke();
          }
          else
          {
            throw "Unknown shape type: " + shapeType;
          }
          current = current.GetNext();
        }
      }

      this.context.restore();
    }
  }

  return BodyDrawerComponent;
});
