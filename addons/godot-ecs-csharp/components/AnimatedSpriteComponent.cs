using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class AnimatedSpriteComponent : AnimatedSprite, IEntityComponent { }

}