using System.ComponentModel.Design;
using System.Diagnostics;
using System.Xml;
using Godot;

namespace GdEcs
{

    public static class GdEcsExtensions
    {

        static GdEcsExtensions() { }

        public static bool IsEntity(this Node node)
        {
            return node is IEntity;
        }

        public static bool IsEntitySystem(this Node node)
        {
            return node is IEntitySystem;
        }

        public static bool IsEntityComponent(this Node node)
        {
            return node is IEntityComponent;
        }

        public static ulong GetEntityId(this IEntity entity)
        {
            return EntityComponentSystem.I.GetEntityId(entity);
        }

        public static EntityComponentStore GetEntityComponentStore(this IEntity entity)
        {
            return EntityComponentSystem.I.GetEntityComponentStore(entity);
        }

        public static void AddEntityComponent(this Node node, IEntityComponent component)
        {
            Debug.Assert(node.IsEntity());
            Debug.Assert(component is Node);
            node.AddChild((Node)component);
        }

        public static void RemoveEntityComponent(this Node node, IEntityComponent component)
        {
            Debug.Assert(node.IsEntity());
            Debug.Assert(component is Node);
            var compNode = (Node)component;
            compNode.QueueFree();
        }

        public static void RemoveEntityComponentsOfType<T>(this Node node)
        {
            Debug.Assert(node.IsEntity());
            node.TraverseChildren(true, (child) =>
            {
                if (child is T && child.GetClosestParentOfType<IEntity>() == node)
                    child.QueueFree();
            });
        }

        public static void TraverseChildren(this Node root, bool recursive, NodeDelegate childCallback)
        {
            foreach (Node child in root.GetChildren())
            {
                if (recursive)
                    TraverseChildren(child, recursive, childCallback);
                childCallback(child);
            }
        }

        public static T? GetClosestParentOfType<T>(this Node child) where T : class
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