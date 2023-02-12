using Godot;

namespace GdEcs
{

    [ExportCustomNode("system", "Node")]
    public class Directional2DToVelocity2DSystem : EntitySystemNode
    {

        [Export]
        public override int EntitySystemPriority { get; set; } = 0;

        protected override void PhysicsProcessEntity(IEntity entity, float delta)
        {
            base.PhysicsProcessEntity(entity, delta);

            var compStore = entity.GetEntityComponentStore();
            var dirComp = compStore.GetFirstComponentOfType<Directional2DComponent>()!;
            var speedComp = compStore.GetFirstComponentOfType<SpeedComponent>()!;
            var velComp = compStore.GetFirstComponentOfType<Velocity2DComponent>()!;

            velComp.Velocity = dirComp.DirVecNormalized * speedComp.Speed;
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            return entity.GetEntityComponentStore().HasComponentsOfAllTypes(
                typeof(Directional2DComponent),
                typeof(SpeedComponent),
                typeof(Velocity2DComponent)
            );
        }

    }

}