using Godot;

namespace GdEcs
{

    [ExportCustomNode]
    public class EntitySpatial : Spatial, IEntity
    {

        public EntityComponentStore ComponentStore { get; }

        public EntitySpatial()
        {
            ComponentStore = new EntityComponentStore(this);
        }

        public override void _Ready()
        {
            base._Ready();
            EntityNodeUtil.I.Ready(this);
        }

    }

}