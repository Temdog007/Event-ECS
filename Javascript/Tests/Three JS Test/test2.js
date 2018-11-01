define(['DrawableComponent', 'Tests/Three JS Test/three'], function(Component, THREE)
{
    class Test2 extends Component
    {
        constructor(entity)
        {
            super(entity);

            var vertices = [
                new THREE.Vector2(0, 0),
                new THREE.Vector2(1, 0),
                new THREE.Vector2(1, 1),
                new THREE.Vector2(0, 1),
            ];
            vertices.forEach(function(vertex)
            {
                vertex.x *= 10;
                vertex.y *= 10;
                vertex.x -= 0.5;
                vertex.y -= 0.5;
            })

            var shape = new THREE.Shape(vertices);
            var geometry = new THREE.ShapeGeometry(shape);

            this.mesh = new THREE.Mesh( geometry );
            this.mesh.position.x = 50;
            this.mesh.position.y = 50;
            this.time = 0;
            Component.Systems.pushEvent("eventAddMesh", this.mesh)

            var comp = this;
            var loader = new THREE.TextureLoader();
            loader.load('ufo.png', function(image){
                comp.mesh2 = new THREE.Mesh( new THREE.ExtrudeGeometry(shape, {depth : 10, bevelEnabled : false}), new THREE.MeshBasicMaterial({map : image}) );
                comp.mesh2.position.x = -50;
                comp.mesh2.position.y = -50;
                Component.Systems.pushEvent("eventAddMesh", comp.mesh2)
            });
        }

        eventUpdate(args)
        {
            this.time += args.dt * 0.001;
            this.mesh.position.x = (this.time % 1) * 50;

            // if(!this.mesh2){return;}
            // this.mesh2.rotation.x += args.dt * 0.001;
            // this.mesh2.rotation.y += args.dt * 0.002;
        }
    }

    return Test2;
});