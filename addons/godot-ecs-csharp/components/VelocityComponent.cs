using Godot;

namespace GdEcs
{

    [ExportCustomNode]
    public class VelocityComponent : Node, IEntityComponent
    {

        [Export]
        public Vector2 Velocity { get; set; }

    }

}