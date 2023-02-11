using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class CollisionShapeComponent : CollisionShape, IEntityComponent { }

}