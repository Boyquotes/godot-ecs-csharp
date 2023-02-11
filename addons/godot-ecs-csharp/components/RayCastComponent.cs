using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class RayCastComponent : RayCast, IEntityComponent { }

}