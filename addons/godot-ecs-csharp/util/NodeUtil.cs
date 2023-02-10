using System.ComponentModel;
using System.Threading.Tasks;
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

        public static T? GetClosestParentOfType<T>(Node child) where T : class
        {
            var parent = child.GetParent<object>();
            if (parent == null)
                return null;
            if (parent is T)
                return (T)parent;
            return GetClosestParentOfType<T>((Node)parent);
        }

    }

}