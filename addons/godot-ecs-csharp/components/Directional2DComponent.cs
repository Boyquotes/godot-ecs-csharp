using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class Directional2DComponent : Node, IEntityComponent
    {

        [Export]
        public Vector2 RawDirVec { get; set; } = Vector2.Zero;

        public Vector2 DirVecNormalized => RawDirVec.Normalized();

        public Direction Direction
        {
            get
            {
                if (RawDirVec.y < 0)
                {
                    if (RawDirVec.x < 0)
                        return Direction.UpLeft;
                    else if (RawDirVec.x > 0)
                        return Direction.UpRight;
                    else
                        return Direction.Up;
                }
                else if (RawDirVec.y > 0)
                {
                    if (RawDirVec.x < 0)
                        return Direction.DownLeft;
                    else if (RawDirVec.x > 0)
                        return Direction.DownRight;
                    else
                        return Direction.Down;
                }
                else
                {
                    if (RawDirVec.x < 0)
                        return Direction.Left;
                    else if (RawDirVec.x > 0)
                        return Direction.Right;
                    else
                        return Direction.None;
                }
            }
        }

    }

}