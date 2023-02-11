using Godot;

namespace GdEcs
{

    [ExportCustomNode("component")]
    public class StringKeyComponent : Node, IEntityComponent
    {

        [Export]
        public string Key { get; set; } = "";

        [Export]
        public NodePath NodePathArg { get; set; } = new NodePath();

        public T? GetNodePathArgNode<T>() where T : Node
        {
            return GetNode<T>(NodePathArg);
        }

    }

}