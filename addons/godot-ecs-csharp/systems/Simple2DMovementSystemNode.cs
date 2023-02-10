using Godot;

namespace GdEcs
{

    [ExportCustomNode("Node")]
    public class Simple2DMovementSystemNode : EntitySystemNode
    {

        protected override void PhysicsProcessEntity(IEntity entity, float delta)
        {
            base.PhysicsProcessEntity(entity, delta);

            var entity2D = (Node2D)entity;
            entity2D.Position += entity.ComponentStore.GetFirstComponentOfType<VelocityComponent>()!.Velocity * delta;
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            return entity is Node2D
                && entity.ComponentStore.HasComponentsOfType<VelocityComponent>();
        }

    }

}