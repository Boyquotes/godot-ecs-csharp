using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    [Tool]
    public class RayCastSnapComponent : RayCastComponent
    {

        [Export]
        public bool OneShot { get; set; } = true;

        public override void _EnterTree()
        {
            base._EnterTree();
            if (CastTo == new Vector3(0, -1, 0))
                CastTo = new Vector3(0, -10000, 0);
        }

    }

}