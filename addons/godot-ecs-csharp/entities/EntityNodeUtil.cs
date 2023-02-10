using System.Diagnostics;
using Godot;

namespace GdEcs
{

    public class EntityNodeUtil : Reference
    {

        private static EntityNodeUtil instance = new EntityNodeUtil();

        public static EntityNodeUtil I => instance;

        private EntityNodeUtil() { }

        public void Ready(Node entity)
        {
            entity.Connect("child_entered_tree", this, nameof(OnChildEnteredTree), new Godot.Collections.Array { entity });
            entity.Connect("child_exiting_tree", this, nameof(OnChildExitedTree), new Godot.Collections.Array { entity });
            entity.Connect("tree_exited", this, nameof(DisconnectFromReady), new Godot.Collections.Array { entity });
            NodeUtil.TraverseChildren(entity, true, (node) => OnChildEnteredTree(node, entity));
        }

        public void DisconnectFromReady(Node entity)
        {
            entity.Disconnect("child_entered_tree", this, nameof(OnChildEnteredTree));
            entity.Disconnect("child_exiting_tree", this, nameof(OnChildExitedTree));
            entity.Disconnect("tree_exited", this, nameof(DisconnectFromReady));
        }

        private void OnChildEnteredTree(Node child, Node entity)
        {
            Debug.Assert(entity is IEntity);
            if (child is IEntityComponent && NodeUtil.GetClosestParentOfType<IEntity>(child) == entity)
            {
                ((IEntity)entity).ComponentStore.AddComponent((IEntityComponent)child);
            }
        }

        private void OnChildExitedTree(Node child, Node entity)
        {
            Debug.Assert(entity is IEntity);
            if (child is IEntityComponent && NodeUtil.GetClosestParentOfType<IEntity>(child) == entity)
            {
                ((IEntity)entity).ComponentStore.RemoveComponent((IEntityComponent)child);
            }
        }

    }

}