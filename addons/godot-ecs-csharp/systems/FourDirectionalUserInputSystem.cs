using Godot;

namespace GdEcs
{

    [ExportCustomNode("system", "Node")]
    public class FourDirectionalUserInputSystem : EntitySystemNode
    {

        protected override void ProcessEntity(IEntity entity, float delta)
        {
            base.ProcessEntity(entity, delta);

            var inputComp = entity.ComponentStore.GetFirstComponentOfType<FourDirectionalUserInputComponent>()!;
            var dirComp = entity.ComponentStore.GetFirstComponentOfType<Directional2DComponent>()!;

            dirComp.RawDirVec = inputComp.RawDirVec;
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            return entity.ComponentStore.HasComponentsOfAllTypes(
                typeof(FourDirectionalUserInputComponent),
                typeof(Directional2DComponent)
            );
        }

    }

}