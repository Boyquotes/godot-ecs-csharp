using Godot;

namespace GdEcs
{

    [ExportCustomNode("entity")]
    public class EntityKinematicBody2D : KinematicBody2D, IEntity
    {

        public ulong EntityId { get; set; }

        public EntityComponentStore ComponentStore { get; }

        public EntityKinematicBody2D()
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