using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class FirstPersonUserInputComponent : Node, IEntityComponent
    {

        [Export]
        public string UpActionName { get; set; } = "move_up";
        [Export]
        public string DownActionName { get; set; } = "move_down";
        [Export]
        public string LeftActionName { get; set; } = "move_left";
        [Export]
        public string RightActionName { get; set; } = "move_right";

        [Export]
        public bool RequireMouseCaptured { get; set; } = false;
        [Export]
        public float MouseSensitivity { get; set; } = 0.005f;

        private float hRotateSinceLast = 0;
        private float vRotateSinceLast = 0;

        public float GetHRotationSinceLastCall()
        {
            var ret = hRotateSinceLast;
            hRotateSinceLast = 0;
            return ret;
        }

        public float GetVRotationSinceLastCall()
        {
            var ret = vRotateSinceLast;
            vRotateSinceLast = 0;
            return ret;
        }

        public Vector3 RawDirVec
        {
            get
            {
                float x = 0, z = 0;
                if (Input.IsActionPressed(UpActionName))
                    z = -1;
                else if (Input.IsActionPressed(DownActionName))
                    z = 1;
                if (Input.IsActionPressed(LeftActionName))
                    x = -1;
                else if (Input.IsActionPressed(RightActionName))
                    x = 1;
                return new Vector3(x, 0, z);
            }
        }

        public Vector3 NormalizedDirVec => RawDirVec.Normalized();

        public Vector3 GetTransformedDirVec(Transform transform)
        {
            var dir = new Vector3();
            if (Input.IsActionPressed(UpActionName))
                dir -= transform.basis.z;
            else if (Input.IsActionPressed(DownActionName))
                dir += transform.basis.z;
            if (Input.IsActionPressed(LeftActionName))
                dir -= transform.basis.x;
            else if (Input.IsActionPressed(RightActionName))
                dir += transform.basis.x;
            return dir;
        }

        public override void _UnhandledInput(InputEvent e)
        {
            base._UnhandledInput(e);
            if (e is InputEventMouseMotion &&
                (!RequireMouseCaptured || Input.MouseMode == Input.MouseModeEnum.Captured))
            {
                var em = (InputEventMouseMotion)e;
                hRotateSinceLast += -em.Relative.x * MouseSensitivity;
                vRotateSinceLast += -em.Relative.y * MouseSensitivity;
            }
        }

    }

}