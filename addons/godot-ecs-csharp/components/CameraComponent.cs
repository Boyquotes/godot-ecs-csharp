using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class CameraComponent : Camera, IEntityComponent { }

}