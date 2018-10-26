require.config({
    baseUrl : "../../..",
    paths :
    {
      drawableComponent : 'Default Components/Interfaces/drawableComponent',
      WorldComponent : "Default Components/Physics/Box2D Components/WorldComponent",
      BodyComponent : "Default Components/Physics/Box2D Components/BodyComponent",
      BodyDrawerComponent : "Default Components/Physics/Box2D Components/BodyDrawerComponent"
    }
  });
  
  require(["Default Components/Physics/Box2d_min"], function()
  {
      function add(t)
      {
        var regex = new RegExp("b2.+");
        for(var i in t)
        {
            if(i.match(regex))
            {
                window[i] = t[i];
            }
            if(typeof t[i] == "object")
            {
                add(t[i]);
            }
        }
      }
      add(Box2D);
    
    
    require([
        'game', 'system', 'systemlist',
        'WorldComponent', 'BodyComponent', 'BodyDrawerComponent'
    ],
    function(Game, _, Systems, WorldComponent, BodyComponent, BodyDrawerComponent)
    {
        var system = Systems.addSystem("Box2d Test");
        var entity = system.createEntity();
        entity.addComponent(WorldComponent);
    
        var ppm = 0.01;
        var mpp = 1 / ppm;
        var width = canvas.width * ppm, height = canvas.height * ppm;
    
        Object.defineProperty(Game, 'ppm', {
            get : function()
            {
                return ppm;
            },
            set : function(value)
            {
                ppm = value;
            }
        });
    
        Object.defineProperty(Game, 'mpp', {
            get : function()
            {
                return mpp;
            },
            set : function(value)
            {
                mpp = value;
            }
        });
    
        for(var i = 0; i < 3; ++i)
        {
            var entity = system.createEntity();
            var arr = entity.addComponents([BodyComponent, BodyDrawerComponent]);
        
            var body = arr[1].body;
            switch(i)
            {
                case 0:
                var shape = new b2PolygonShape();
                shape.SetAsBox(4, 0.5);
                var fixtureDef = new b2FixtureDef();
                fixtureDef.shape = shape;
                body.CreateFixture(fixtureDef);
                body.SetPosition(new b2Vec2(width / 2, height - 0.5));
                break;
                case 1:
                var shape = new b2CircleShape();
                shape.SetRadius(1);
                var fixtureDef = new b2FixtureDef();
                fixtureDef.restitution = 0.75;
                fixtureDef.shape = shape;
                body.CreateFixture(fixtureDef);
                body.SetPosition(new b2Vec2(width / 2, height / 2));
                body.SetType(b2Body.b2_dynamicBody);
                break;
                case 2:
                var shape = new b2PolygonShape.AsEdge(new b2Vec2(2, 2), new b2Vec2(1, 1));
                var fixtureDef = new b2FixtureDef();
                fixtureDef.shape = shape;
                console.log(fixtureDef);
                body.CreateFixture(fixtureDef);
                break;
            }
        }
    });
});
  