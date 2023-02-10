using Godot;

namespace GdEcs
{

    [ExportCustomNode("system", "Node")]
    public class SimpleKinematicBody2DMovementSystem : EntitySystemNode
    {

        [Export]
        public override int EntitySystemPriority { get; set; } = 0;

        [Export]
        public Vector2 UpDirection { get; set; } = Vector2.Zero;

        [Export]
        public bool StopOnSlope { get; set; } = false;

        protected override void PhysicsProcessEntity(IEntity entity, float delta)
        {
            base.PhysicsProcessEntity(entity, delta);

            var entityT = (KinematicBody2D)entity;
            var compStore = entity.GetEntityComponentStore();
            var velComp = compStore.GetFirstComponentOfType<Velocity2DComponent>()!;

            entityT.MoveAndSlide(velComp.Velocity, UpDirection, StopOnSlope);
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            return entity is KinematicBody2D
                && entity.GetEntityComponentStore().HasComponentsOfType<Velocity2DComponent>();
        }

    }

}