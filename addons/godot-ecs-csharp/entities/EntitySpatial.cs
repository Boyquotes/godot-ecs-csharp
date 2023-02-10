using Godot;

namespace GdEcs
{

    [ExportCustomNode("entity")]
    public class EntitySpatial : Spatial, IEntity
    {

        public ulong EntityId { get; set; }

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