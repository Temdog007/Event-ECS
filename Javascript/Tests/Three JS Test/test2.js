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
                new THREE.Vector2(0.5, 1),
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

            this.mesh = new THREE.Mesh( geometry, new THREE.MeshNormalMaterial()  );
            this.mesh.position.x = 50;
            this.mesh.position.y = 50;
            this.time = 0;

            var Systems = require("systemlist");
            Systems.pushEvent("eventAddMesh", this.mesh)

            this.mesh2 = new THREE.Mesh( new THREE.ExtrudeGeometry(shape, {depth : 10, bevelEnabled : false}), new THREE.MeshNormalMaterial() );
            this.mesh2.position.x = 50;
            this.mesh2.position.y = -50;
            Systems.pushEvent("eventAddMesh", this.mesh2);

            var verts = [];
            for(var angle = 0; angle <= Math.PI * 0.5; angle += Math.PI * 0.5 / 16)
            {
                verts.push(new THREE.Vector2(Math.cos(angle) * 20, Math.sin(angle) * 20));
            }
            verts.push(new THREE.Vector2(-20, 20));
            verts.push(new THREE.Vector2(-20, 0));
            verts.push(new THREE.Vector2(0,0));
            var shape = new THREE.Shape(verts);

            var mesh3 = new THREE.Mesh( new THREE.ExtrudeGeometry(shape, {depth : 10, bevelEnabled : false}), new THREE.MeshNormalMaterial());
            mesh3.position.x = -50;
            mesh3.position.y = -25;
            mesh3.position.z = 25;
            Systems.pushEvent("eventAddMesh", mesh3);

            mesh3 = new THREE.Mesh( new THREE.ExtrudeGeometry(shape, {depth : 5, bevelEnabled : true, bevelThickness : 1, bevelSize : 1, bevelSegments : 5}), new THREE.MeshNormalMaterial());
            mesh3.position.x = 50;
            mesh3.position.y = -25;
            mesh3.position.z = 25;
            Systems.pushEvent("eventAddMesh", mesh3);
        }

        eventUpdate(args)
        {
            this.time += args.dt * 0.001;
            this.mesh.position.x = (this.time % 1) * 50;

            if(!this.mesh2){return;}
            this.mesh2.rotation.x += args.dt * 0.001;
            this.mesh2.rotation.y += args.dt * 0.002;
        }
    }

    return Test2;
});