require.config({
    baseUrl : '../../',
    paths :
    {
      loadTexture : "Tests/Shader Test/loadTexture",
      createSquare : "Tests/Shader Test/createSquare",
      DrawableComponent : "Default Components/Interfaces/DrawableComponent",
    }
  });

  require(['DrawableComponent', 'game', 'systemlist', 'system', 'Tests/Three JS Test/three'], function(Component, Game, Systems, _, THREE)
  {
    function onLoad(image)
    {
        this.camera = new THREE.PerspectiveCamera( 70, window.innerWidth / window.innerHeight, 1, 2000 );
        this.camera.position.z = 100;

        this.scene = new THREE.Scene();

        var geometry = new THREE.SphereGeometry(20);
        var material = new THREE.MeshBasicMaterial({map : image, color : "white"});

        this.meshes = [];

        var mesh = new THREE.Mesh( geometry, material );
        mesh.position.x = 50;
        this.scene.add( mesh );
        this.meshes.push(mesh);

        geometry = new THREE.SphereGeometry(20);
        material = new THREE.MeshNormalMaterial();

        mesh = new THREE.Mesh( geometry, material );
        this.scene.add( mesh );
        this.meshes.push(mesh);

        this.renderer = new THREE.WebGLRenderer( { antialias: true } );
        this.renderer.setSize( window.innerWidth, window.innerHeight );

        // document.body.appendChild( this.renderer.domElement );
        // canvas.style.display = "none";

        this.speed = 1;
    }

    class ThreeComponent extends Component
    {
        constructor(entity)
        {
            super(entity);
            
            var loader = new THREE.TextureLoader();
            loader.load('ufo.png', onLoad.bind(this));
        }

        eventUpdate(args)
        {
            if(!this.meshes){return;}
            this.meshes.forEach(function(mesh)
            {
                mesh.rotation.x += args.dt * 0.001;
                mesh.rotation.y += args.dt * 0.002;
            });
        }

        eventDraw()
        {
            if(!this.renderer){return;}
            this.renderer.render(this.scene, this.camera);
            this.context.drawImage(this.renderer.domElement, 0, 0, this.renderer.domElement.width, this.renderer.domElement.height, 0, 0, this.canvas.width, this.canvas.height);
        }

        eventKeyDown(args)
        {
            if(!this.camera){return;}
            if(args.key == "s")
            {
                this.camera.position.y -= this.speed;
            }
            else if(args.key == "w")
            {
                this.camera.position.y += this.speed;
            }
            else if(args.key == "a")
            {
                this.camera.position.x -= this.speed;
            }
            else if(args.key == "d")
            {
                this.camera.position.x += this.speed;
            }
            else if(args.key == "ArrowUp")
            {
                this.camera.position.z -= this.speed;
            }
            else if(args.key == "ArrowDown")
            {
                this.camera.position.z += this.speed;
            }
            else if(args.key == "Enter")
            {
                console.log(this.camera.position);
            }
        }
    }

    var system = Systems.addSystem("Three");
    var entity = system.createEntity();
    entity.addComponent(ThreeComponent);
  });