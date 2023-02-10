using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

namespace GdEcs
{

    public enum EntityUpdateType
    {
        Update, Add, Remove,
    }

    public struct EntityUpdateEntry
    {
        public readonly IEntity Entity;
        public readonly EntityUpdateType EntityUpdateType;

        public EntityUpdateEntry(IEntity entity, EntityUpdateType entityUpdateType)
        {
            this.Entity = entity;
            this.EntityUpdateType = entityUpdateType;
        }
    }

    public struct SystemUpdateEntry
    {
        public readonly IEntitySystem System;
        public readonly bool Remove;

        public SystemUpdateEntry(IEntitySystem system, bool remove)
        {
            this.System = system;
            this.Remove = remove;
        }
    }

    [ExportCustomNode("KeyBezierHandle")]
    public class EntitySystemManagerNode : Node
    {

        public const ulong INVALID_ENTITY_ID = 0;

        public event EntityDelegate EntityAdded = delegate { };
        public event EntityDelegate EntityRemoved = delegate { };
        public event EntitySystemDelegate EntitySystemAdded = delegate { };
        public event EntitySystemDelegate EntitySystemRemoved = delegate { };

        private Dictionary<ulong, IEntity> entities = new Dictionary<ulong, IEntity>();
        private PriorityList<IEntitySystem> systems = new PriorityList<IEntitySystem>(
            (a, b) => a.EntitySystemPriority > b.EntitySystemPriority
                ? -1
                : (a.EntitySystemPriority < b.EntitySystemPriority ? 1 : 0));

        private List<SystemUpdateEntry> systemUpdateEntries = new List<SystemUpdateEntry>();
        private List<EntityUpdateEntry> entityUpdateEntries = new List<EntityUpdateEntry>();

        private ulong nextEntityId = 1;

        public override void _Ready()
        {
            base._Ready();

            OnEnteredTree();

            Connect("child_entered_tree", this, nameof(OnChildEnteredTree));
            Connect("child_exiting_tree", this, nameof(OnChildExitingTree));
            Connect("tree_exited", this, nameof(OnExitedTree));
            NodeUtil.TraverseChildren(this, true, OnChildEnteredTree);
        }

        public T? GetEntity<T>(ulong entityId) where T : class, IEntity
        {
            if (entities.ContainsKey(entityId))
                return (T)entities[entityId];
            return null;
        }

        public IEnumerable<IEntity> GetEntities()
        {
            return entities.Values.AsEnumerable();
        }

        public bool HasEntity(ulong entityId)
        {
            return GetEntity<IEntity>(entityId) != null;
        }

        public void AddEntity(IEntity entity)
        {
            entityUpdateEntries.Add(new EntityUpdateEntry(entity, EntityUpdateType.Add));
        }

        public void RemoveEntity(ulong entityId)
        {
            entityUpdateEntries.Add(new EntityUpdateEntry(entities[entityId], EntityUpdateType.Remove));
        }

        public void AddSystem(IEntitySystem system)
        {
            systemUpdateEntries.Add(new SystemUpdateEntry(system, false));
        }

        public void RemoveSystem(IEntitySystem system)
        {
            systemUpdateEntries.Add(new SystemUpdateEntry(system, true));
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            foreach (var entry in entityUpdateEntries)
            {
                switch (entry.EntityUpdateType)
                {
                    case EntityUpdateType.Add:
                        Debug.Assert(!entities.Values.Contains(entry.Entity));
                        entry.Entity.EntityId = NextEntityId();
                        entities.Add(entry.Entity.EntityId, entry.Entity);
                        entry.Entity.ComponentStore.ComponentsChanged += OnEntityComponentsChanged;
                        EntityAdded(entry.Entity);
                        break;
                    case EntityUpdateType.Remove:
                        Debug.Assert(entities.Values.Contains(entry.Entity));
                        entry.Entity.ComponentStore.ComponentsChanged -= OnEntityComponentsChanged;
                        entities.Remove(entry.Entity.EntityId);
                        EntityRemoved(entry.Entity);
                        entry.Entity.EntityId = 0;
                        break;
                }
                foreach (var system in systems)
                    system.RefreshProcessesEntity(entry.Entity);
            }
            entityUpdateEntries.Clear();

            foreach (var entry in systemUpdateEntries)
            {
                if (entry.Remove)
                {
                    Debug.Assert(systems.Contains(entry.System));
                    systems.Remove(entry.System);
                    EntitySystemRemoved(entry.System);
                }
                else
                {
                    Debug.Assert(!systems.Contains(entry.System));
                    systems.AddPrioritized(entry.System);
                    EntitySystemAdded(entry.System);
                    foreach (var ent in entities.Values)
                        entry.System.RefreshProcessesEntity(ent);
                }
            }
            systemUpdateEntries.Clear();

            foreach (var system in systems)
            {
                system.DoProcess(delta);
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);

            foreach (var system in systems)
                system.DoPhysicsProcess(delta);
        }

        private void OnEntityComponentsChanged(IEntity entity)
        {
            // TODO: only if not already an entry in there
            entityUpdateEntries.Add(new EntityUpdateEntry(entity, EntityUpdateType.Update));
        }

        private void OnChildEnteredTree(Node node)
        {
            if (node is IEntity)
            {
                var ent = (IEntity)node;
                AddEntity(ent);
            }
            else if (node is IEntitySystem)
            {
                var sys = (IEntitySystem)node;
                AddSystem(sys);
            }
        }

        private void OnChildExitingTree(Node node)
        {
            if (node is IEntity)
            {
                var ent = (IEntity)node;
                RemoveEntity(ent.EntityId);
            }
            else if (node is IEntitySystem)
            {
                var sys = (IEntitySystem)node;
                RemoveSystem(sys);
            }
        }

        private void OnEnteredTree()
        {
            Debug.Assert(NodeUtil.GetClosestParentOfType<EntitySystemManagerNode>(this) == null,
                "EntitySystemManagerNode cannot be in the subtree of another EntitySystemManagerNode");
        }

        private void OnExitedTree()
        {
            Disconnect("child_entered_tree", this, nameof(OnChildEnteredTree));
            Disconnect("child_exiting_tree", this, nameof(OnChildExitingTree));
            Disconnect("tree_exited", this, nameof(OnExitedTree));
        }

        private ulong NextEntityId()
        {
            return nextEntityId++;
        }

    }

}