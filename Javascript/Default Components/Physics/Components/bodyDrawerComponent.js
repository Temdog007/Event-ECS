define(['box2d', 'drawableComponent', 'game'],
function(box2d, Component, Game)
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
      if(type == box2d.b2_dynamicBody)
      {
        return this.get("dynamicColor");
      }
      else if(type == box2d.b2_kinematicBody)
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
        while(current && current.a)
        {
          var shapeType = current.GetType();
          if(shapeType == box2d.b2Shape.e_circle)
          {
            var shape = box2d.wrapPointer(current.GetShape().a, box2d.b2CircleShape);
            var center = shape.get_m_p();
            center = body.GetWorldPoint(center);
            this.context.beginPath();
            this.context.arc(
              center.get_x()*Game.mpp,
              center.get_y()*Game.mpp,
              shape.get_m_radius()*Game.mpp,
              0, Math.PI * 2);
            this.context.stroke();
          }
          else if (shapeType == box2d.b2Shape.e_edge)
          {
            var shape = box2d.wrapPointer(current.GetShape().a, box2d.b2EdgeShape);
            this.context.beginPath();

            var vertex = body.GetWorldPoint(shape.get_m_vertex1());
            var x = vertex.get_x(), y = vertex.get_y();
            x *= Game.mpp;
            y *= Game.mpp;
            this.context.moveTo(x,y);

            vertex = body.GetWorldPoint(shape.get_m_vertex2());
            x = vertex.get_x(); y = vertex.get_y();
            x *= Game.mpp;
            y *= Game.mpp;
            this.context.lineTo(x,y);

            this.context.closePath();
            this.context.stroke();
          }
          else if (shapeType == box2d.b2Shape.e_chain)
          {
            throw "Can't draw Chain Shape because can't create them in javascript";
          }
          else if(shapeType == box2d.b2Shape.e_polygon)
          {
            var shape = box2d.wrapPointer(current.GetShape().a, box2d.b2PolygonShape);
            var count = shape.GetVertexCount();
            this.context.beginPath();
            for(var i = 0; i < count; ++i)
            {
              var vertex = body.GetWorldPoint(shape.GetVertex(i));
              var x = vertex.get_x(), y = vertex.get_y();
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
