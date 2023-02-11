using Godot;

namespace GdEcs
{

    [ExportCustomNode("component", "RayCast")]
    public class RayCastSnapComponent : RayCastComponent
    {

        [Export]
        public bool OneShot { get; set; } = true;

    }

}