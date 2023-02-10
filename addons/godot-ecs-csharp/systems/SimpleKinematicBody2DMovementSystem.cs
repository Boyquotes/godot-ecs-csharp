using Godot;

namespace GdEcs
{

    [ExportCustomNode("system", "Node")]
    public class SimpleKinematicBody2DMovementSystem : EntitySystemNode
    {

        protected override void PhysicsProcessEntity(IEntity entity, float delta)
        {
            base.PhysicsProcessEntity(entity, delta);

            var entityT = (KinematicBody2D)entity;
            var velComp = entity.ComponentStore.GetFirstComponentOfType<Velocity2DComponent>()!;

            entityT.MoveAndSlide(velComp.Velocity);
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            return entity is KinematicBody2D
                && entity.ComponentStore.HasComponentsOfType<Velocity2DComponent>();
        }

    }

}