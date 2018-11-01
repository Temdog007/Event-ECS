define(['DrawableComponent', 'Tests/Three JS Test/three'], function(Component, THREE)
{
    class Test2 extends Component
    {
        constructor(entity)
        {
            super(entity);

            var vertices = [
                new THREE.Vector2(0, 0),
                new THREE.Vector2(0, 1),
                new THREE.Vector2(1, 1),
                new THREE.Vector2(1, 0),
            ]

            var geometry = new THREE.ShapeGeometry(new THREE.Shape(vertices));
            var material = new THREE.MeshBasicMaterial();
    
            this.mesh = new THREE.Mesh( geometry, material );
            this.mesh.position.x = 50;
            this.mesh.position.y = 50;
            this.time = 0;
            Component.Systems.pushEvent("eventAddMesh", this.mesh)
        }

        eventUpdate(args)
        {
            this.time += args.dt * 0.001;
            this.mesh.position.x = (this.time % 1) * 50;
        }
    }

    return Test2;
});