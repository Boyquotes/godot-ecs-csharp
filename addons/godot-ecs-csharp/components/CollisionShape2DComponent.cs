using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class CollisionShape2DComponent : CollisionShape2D, IEntityComponent { }

}