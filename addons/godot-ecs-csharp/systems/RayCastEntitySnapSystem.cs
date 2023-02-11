using Godot;

namespace GdEcs
{

    [ExportCustomNode("system", "Node")]
    public class RayCastEntitySnapSystem : EntitySystemNode
    {

        [Export]
        public override int EntitySystemPriority { get; set; } = 0;

        protected override void ProcessEntity(IEntity entity, float delta)
        {
            base.ProcessEntity(entity, delta);

            var entityT = (Spatial)entity;
            var rayComp = entity.GetEntityComponentStore().GetFirstComponentOfType<RayCastSnapComponent>()!;

            var obj = rayComp.GetCollider() as Spatial;
            if (obj != null)
                entityT.GlobalTranslation = obj.GlobalTranslation - rayComp.Translation;

            if (rayComp.OneShot)
                entityT.RemoveEntityComponent(rayComp);
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            return entity is Spatial
                && entity.GetEntityComponentStore().HasComponentsOfType<RayCastSnapComponent>();
        }

    }

}