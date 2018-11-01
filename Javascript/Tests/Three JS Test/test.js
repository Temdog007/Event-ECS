require.config({
    baseUrl : '../../',
    paths :
    {
      loadTexture : "Tests/Shader Test/loadTexture",
      createSquare : "Tests/Shader Test/createSquare",
      DrawableComponent : "Default Components/Interfaces/DrawableComponent",
    }
  });

  require(['DrawableComponent', 'game', 'systemlist', 'system', 'Tests/Three JS Test/three', 'Tests/Three JS Test/test2'], function(Component, _, Systems, _, THREE, Test2)
  {
    function onLoad(image)
    {
        this.camera = new THREE.PerspectiveCamera( 70, window.innerWidth / window.innerHeight);
        this.camera.position.z = 100;

        var geometry = new THREE.SphereGeometry(20);
        var material = new THREE.MeshBasicMaterial({map : image, color : "cyan"});

        this.meshes = [];

        var mesh = new THREE.Mesh( geometry, material );
        mesh.position.x = 50;
        this.scene.add( mesh );
        this.meshes.push(mesh);

        geometry = new THREE.BoxGeometry(20, 20, 20);
        material = new THREE.MeshNormalMaterial();

        mesh = new THREE.Mesh( geometry, material );
        this.scene.add( mesh );
        this.meshes.push(mesh);

        geometry = new THREE.ConeGeometry(20, 20);
        material = new THREE.MeshNormalMaterial();

        mesh = new THREE.Mesh( geometry, material );
        mesh.position.x = -50;
        this.scene.add( mesh );
        this.meshes.push(mesh);

        geometry = new THREE.CircleGeometry(20, 20);
        material = new THREE.MeshNormalMaterial();

        mesh = new THREE.Mesh( geometry, material );
        mesh.position.x = -100;
        this.scene.add( mesh );
        this.meshes.push(mesh);

        geometry = new THREE.RingGeometry(20);
        material = new THREE.MeshNormalMaterial();

        mesh = new THREE.Mesh( geometry, material );
        mesh.position.x = 100;
        this.scene.add( mesh );
        this.meshes.push(mesh);

        this.renderer = new THREE.WebGLRenderer( { antialias: true } );
        this.renderer.setSize( window.innerWidth, window.innerHeight );
        document.body.appendChild(this.renderer.domElement);

        this.speed = 1;
    }

    class ThreeComponent extends Component
    {
        constructor(entity)
        {
            super(entity);

            this.scene = new THREE.Scene();
            
            var loader = new THREE.TextureLoader();
            loader.load('ufo.png', onLoad.bind(this));
        }

        addMesh()
        {
            this.last = this.last || 0;

            var geometry = new THREE.BoxGeometry(20, 20, 20);
            var material = new THREE.MeshNormalMaterial();

            var mesh = new THREE.Mesh( geometry, material );
            mesh.position.y = this.last++ * 10;
            this.scene.add( mesh );
            this.meshes.push(mesh);
        }

        eventAddMesh(mesh)
        {
            this.scene.add(mesh);
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
            // this.context.drawImage(this.renderer.domElement, 0, 0, this.renderer.domElement.width, this.renderer.domElement.height, 0, 0, this.canvas.width, this.canvas.height);
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
            else if(args.key == "F1")
            {
                this.addMesh();
            }
        }
    }

    var system = Systems.addSystem("Three");
    var entity = system.createEntity();
    entity.addComponents([ThreeComponent, Test2]);
  });