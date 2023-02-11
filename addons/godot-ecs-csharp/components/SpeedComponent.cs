using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class SpeedComponent : Node, IEntityComponent
    {

        [Export]
        public float Speed { get; set; } = 1000;

    }

}