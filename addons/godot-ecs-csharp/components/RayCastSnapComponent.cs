using Godot;

namespace GdEcs
{

    [Tool]
    [ExportCustomNode("component")]
    public class RayCastSnapComponent : RayCastComponent
    {

        [Export]
        public bool OneShot { get; set; } = true;

        public override void _EnterTree()
        {
            base._EnterTree();
            if (!Engine.EditorHint)
                return;
            if (CastTo == new Vector3(0, -1, 0))
                CastTo = new Vector3(0, -10000, 0);
        }

    }

}