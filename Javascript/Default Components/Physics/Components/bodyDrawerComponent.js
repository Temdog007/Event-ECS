define(['box2d', 'drawableComponent', 'Game'],
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

    doDraw()
    {
      Game.context.save();

      Game.context.globalAlpha = this.alpha;
      Game.context.strokeStyle = this.color;

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
            Game.context.beginPath();
            Game.context.arc(center.get_x(), center.get_y(), shape.get_m_radius(), 0, Math.PI * 2);
            Game.context.stroke();
          }
          else if (shapeType == box2d.b2Shape.e_edge)
          {
            var shape = box2d.wrapPointer(current.GetShape().a, box2d.b2EdgeShape);
            Game.context.beginPath();

            var vertex = body.GetWorldPoint(shape.get_m_vertex1());
            var x = vertex.get_x(), y = vertex.get_y();
            Game.context.moveTo(x,y);

            vertex = body.GetWorldPoint(shape.get_m_vertex2());
            x = vertex.get_x(); y = vertex.get_y();
            Game.context.lineTo(x,y);

            Game.context.closePath();
            Game.context.stroke();
          }
          else if (shapeType == box2d.b2Shape.e_chain)
          {
            var shape = box2d.wrapPointer(current.GetShape().a, box2d.b2ChainShape);
            var count = shape.GetVertexCount();
            Game.context.beginPath();
            for(var i = 0; i < count; ++i)
            {
              var vertex = body.GetWorldPoint(shape.GetVertex(i));
              var x = vertex.get_x(), y = vertex.get_y();
              if(i == 0)
              {
                Game.context.moveTo(x,y);
              }
              else
              {
                Game.context.lineTo(x,y);
              }
            }
            Game.context.closePath();
            Game.context.stroke();
          }
          else if(shapeType == box2d.b2Shape.e_polygon)
          {
            var shape = box2d.wrapPointer(current.GetShape().a, box2d.b2PolygonShape);
            var count = shape.GetVertexCount();
            Game.context.beginPath();
            for(var i = 0; i < count; ++i)
            {
              var vertex = body.GetWorldPoint(shape.GetVertex(i));
              var x = vertex.get_x(), y = vertex.get_y();
              if(i == 0)
              {
                Game.context.moveTo(x,y);
              }
              else
              {
                Game.context.lineTo(x,y);
              }
            }
            Game.context.closePath();
            Game.context.stroke();
          }
          else
          {
            throw "Unknown shape type: " + shapeType;
          }
          current = current.GetNext();
        }
      }

      Game.context.restore();
    }
  }

  return BodyDrawerComponent;
});
