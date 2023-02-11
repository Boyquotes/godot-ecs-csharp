using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class Camera2DComponent : Camera2D, IEntityComponent { }

}