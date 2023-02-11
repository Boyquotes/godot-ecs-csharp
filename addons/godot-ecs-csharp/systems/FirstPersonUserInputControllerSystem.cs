using System.Diagnostics;
using Godot;

namespace GdEcs
{

    [ExportCustomNode("system", "Node")]
    public class FirstPersonUserInputControllerSystem : EntitySystemNode
    {

        public const string FPS_CAM_PIVOT_KEY = "FPSCamPivot";

        [Export]
        public override int EntitySystemPriority { get; set; } = 0;

        [Export]
        public Vector2 VerticalViewRotationClamp { get; set; } = new Vector2(-1, 1);

        protected override void ProcessEntity(IEntity entity, float delta)
        {
            base.ProcessEntity(entity, delta);

            var compStore = entity.GetEntityComponentStore();
            var entityT = (Spatial)entity;
            var fpsComp = compStore.GetFirstComponentOfType<FirstPersonUserInputComponent>()!;
            var velComp = compStore.GetFirstComponentOfType<Velocity3DComponent>()!;
            var speedComp = compStore.GetFirstComponentOfType<SpeedComponent>()!;
            var camPivotKeyComp = compStore.GetStringKeyComponentMatching(FPS_CAM_PIVOT_KEY);

            var moveVel = fpsComp.GetTransformedDirVec(entityT.GlobalTransform) * speedComp.Speed * delta;
            velComp.Velocity = new Vector3(moveVel.x, velComp.Velocity.y, moveVel.z);
            entityT.RotateY(fpsComp.GetHRotationSinceLastCall());

            if (camPivotKeyComp != null)
            {
                var camPivot = camPivotKeyComp.GetNodePathArgNode<Spatial>();
                if (camPivot != null)
                {
                    camPivot.RotateX(fpsComp.GetVRotationSinceLastCall());
                    camPivot.Rotation = new Vector3(
                        Mathf.Clamp(camPivot.Rotation.x,
                            VerticalViewRotationClamp.x,
                            VerticalViewRotationClamp.y),
                        camPivot.Rotation.y,
                        camPivot.Rotation.z);
                }
            }
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            var compStore = entity.GetEntityComponentStore();
            return entity is Spatial
                    && compStore.HasComponentsOfType<FirstPersonUserInputComponent>()
                    && compStore.HasComponentsOfType<Velocity3DComponent>()
                    && compStore.HasComponentsOfType<SpeedComponent>();
        }

    }

}