using Godot;

namespace GdEcs
{

    [ExportCustomNode("entity")]
    public class EntityNode : Node, IEntity
    {

        public ulong EntityId { get; set; }

        public EntityComponentStore ComponentStore { get; }

        public EntityNode()
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