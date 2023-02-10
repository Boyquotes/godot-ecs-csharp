using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class FourDirectionalUserInputComponent : Node, IEntityComponent
    {

        [Export]
        public string UpActionName { get; set; } = "move_up";
        [Export]
        public string DownActionName { get; set; } = "move_down";
        [Export]
        public string LeftActionName { get; set; } = "move_left";
        [Export]
        public string RightActionName { get; set; } = "move_right";

        public Vector2 RawDirVec
        {
            get
            {
                float x = 0, y = 0;
                if (Input.IsActionPressed(UpActionName))
                    y = -1;
                else if (Input.IsActionPressed(DownActionName))
                    y = 1;
                if (Input.IsActionPressed(LeftActionName))
                    x = -1;
                else if (Input.IsActionPressed(RightActionName))
                    x = 1;
                return new Vector2(x, y);
            }
        }

        public Vector2 NormalizedDirVec => RawDirVec.Normalized();

        public bool DirVecIsZero => RawDirVec == Vector2.Zero;

    }

}