define(['DrawableComponent', 'Tests/Three JS Test/three'], function(Component, THREE)
{
    class Test2 extends Component
    {
        constructor(entity)
        {
            super(entity);

            var geometry = new THREE.SphereGeometry(20);
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