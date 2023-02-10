using Godot;

namespace GdEcs
{

    [ExportCustomNode]
    public class EntityNode : Node, IEntity
    {

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