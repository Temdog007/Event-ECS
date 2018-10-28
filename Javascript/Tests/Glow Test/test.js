require.config({
    baseUrl : "../../",
    paths :
    {
      drawableComponent : "Default Components/Interfaces/drawableComponent"
    }
  });
  
  require(['system', 'systemlist', 'drawableComponent', 'game'], function(System, Systems, Component)
  {
    class Rect extends Component
    {
      constructor(entity)
      {
        super(entity);
        this.setDefaults({
          x : 0,
          y : 0,
          r : 0,
          w : 100,
          h : 100,
          rotate : true,
          color : "black",
          center : false
        });
      }
  
      eventDraw()
      {
        var data = this.data;
        
        var context = this.context;
        context.shadowColor =  data.color;
        context.shadowOffsetX = 0;
        context.shadowOffsetY = 0;
        context.shadowBlur = 100;
        context.fillStyle = data.color;
        context.lineWidth = 30;
        // context.textBaseline = "top";
        // context.font = "49px comic-sans";
        // context.fillText("Neon", 0, 0);
        // context.fillRect(50, 50, 50, 50);
        context.strokeStyle = data.color;
        context.moveTo(50, 50);
        context.lineTo(100, 100);
        context.lineTo(110, 110);
        context.stroke();
      }
    }
  
    var system = Systems.addSystem("Test");
  
    function makeEntity()
    {
      var entity = system.createEntity();
      entity.addComponent(Rect);
      return entity;
    }
  
    var cx = canvas.width / 2, cy = canvas.height / 2;
  
    var entity1 = makeEntity();
    entity1.set("color", "blue");
    entity1.set("x", cx);
    entity1.set("y", cy);
  });
  