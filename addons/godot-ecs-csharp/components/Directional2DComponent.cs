using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class Directional2DComponent : Node, IEntityComponent
    {

        [Export]
        public Vector2 RawDirVec { get; set; } = Vector2.Zero;

        public Vector2 DirVecNormalized => RawDirVec.Normalized();

    }

}