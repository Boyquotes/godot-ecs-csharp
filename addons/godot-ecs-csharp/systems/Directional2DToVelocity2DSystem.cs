using Godot;

namespace GdEcs
{

    [ExportCustomNode("system", "Node")]
    public class Directional2DToVelocity2DSystem : EntitySystemNode
    {

        protected override void PhysicsProcessEntity(IEntity entity, float delta)
        {
            base.PhysicsProcessEntity(entity, delta);

            var dirComp = entity.ComponentStore.GetFirstComponentOfType<Directional2DComponent>()!;
            var speedComp = entity.ComponentStore.GetFirstComponentOfType<SpeedComponent>()!;
            var velComp = entity.ComponentStore.GetFirstComponentOfType<Velocity2DComponent>()!;

            velComp.Velocity = dirComp.DirVecNormalized * speedComp.Speed * delta;
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            return entity.ComponentStore.HasComponentsOfAllTypes(
                typeof(Directional2DComponent),
                typeof(SpeedComponent),
                typeof(Velocity2DComponent)
            );
        }

    }

}