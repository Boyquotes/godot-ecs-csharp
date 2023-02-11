using Godot;

namespace GdEcs
{

    [ExportCustomNode("system", "Node")]
    public class SimpleKinematicBody3DMovementSystem : EntitySystemNode
    {

        [Export]
        public override int EntitySystemPriority { get; set; } = 0;

        [Export]
        public Vector3 UpDirection { get; set; } = Vector3.Up;

        [Export]
        public bool StopOnSlope { get; set; } = false;

        [Export]
        public Vector3 Gravity { get; set; } = new Vector3(0, 0, 0);

        protected override void PhysicsProcessEntity(IEntity entity, float delta)
        {
            base.PhysicsProcessEntity(entity, delta);

            var entityT = (KinematicBody)entity;
            var compStore = entity.GetEntityComponentStore();
            var velComp = compStore.GetFirstComponentOfType<Velocity3DComponent>()!;

            velComp.Velocity = velComp.Velocity + Gravity * delta;

            entityT.MoveAndSlide(velComp.Velocity, UpDirection, StopOnSlope);
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            return entity is KinematicBody
                && entity.GetEntityComponentStore().HasComponentsOfType<Velocity3DComponent>();
        }

    }

}