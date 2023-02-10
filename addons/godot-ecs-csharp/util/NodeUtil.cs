using Godot;

namespace GdEcs
{

    public sealed class NodeUtil
    {

        private NodeUtil() { }

        public static void TraverseChildren(Node root, bool recursive, NodeDelegate childCallback)
        {
            foreach (Node child in root.GetChildren())
            {
                if (recursive)
                    TraverseChildren(child, recursive, childCallback);
                childCallback(child);
            }
        }

    }

}