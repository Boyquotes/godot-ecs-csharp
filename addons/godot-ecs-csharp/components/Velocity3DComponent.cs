using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class Velocity3DComponent : Node, IEntityComponent
    {

        [Export]
        public Vector3 Velocity { get; set; } = Vector3.Zero;

    }

}