using Godot;

namespace GdEcs
{

    [ExportCustomNode("entity")]
    public class EntityNode2D : Node2D, IEntity
    {

        public ulong EntityId { get; set; }

        public EntityComponentStore ComponentStore { get; }

        public EntityNode2D()
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