using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class RayCast2DComponent : RayCast2D, IEntityComponent { }

}