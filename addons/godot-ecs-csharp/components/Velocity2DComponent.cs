using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class Velocity2DComponent : Node, IEntityComponent
    {

        [Export]
        public Vector2 Velocity { get; set; } = Vector2.Zero;

    }

}