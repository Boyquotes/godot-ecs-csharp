using System.Collections.Generic;
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

    [ExportCustomNode]
    public class EntitySystemManagerNode : Node
    {

        private List<IEntity> entities = new List<IEntity>();
        private List<IEntitySystem> systems = new List<IEntitySystem>();

        private List<SystemUpdateEntry> systemUpdateEntries = new List<SystemUpdateEntry>();
        private List<EntityUpdateEntry> entityUpdateEntries = new List<EntityUpdateEntry>();

        public override void _Ready()
        {
            base._Ready();

            Connect("child_entered_tree", this, nameof(OnChildEnteredTree));
            Connect("child_exiting_tree", this, nameof(OnChildExitingTree));
            Connect("tree_exited", this, nameof(OnExitedTree));
            NodeUtil.TraverseChildren(this, true, OnChildEnteredTree);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            foreach (var entry in entityUpdateEntries)
            {
                switch (entry.EntityUpdateType)
                {
                    case EntityUpdateType.Add:
                        entities.Add(entry.Entity);
                        entry.Entity.ComponentStore.ComponentsChanged += OnEntityComponentsChanged;
                        break;
                    case EntityUpdateType.Remove:
                        entry.Entity.ComponentStore.ComponentsChanged -= OnEntityComponentsChanged;
                        entities.Remove(entry.Entity);
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
                    systems.Remove(entry.System);
                }
                else
                {
                    systems.Add(entry.System);
                    foreach (var ent in entities)
                        entry.System.RefreshProcessesEntity(ent);
                }
            }
            systemUpdateEntries.Clear();

            foreach (var system in systems)
                system.DoProcess(delta);
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
                entityUpdateEntries.Add(new EntityUpdateEntry(ent, EntityUpdateType.Add));
            }
            else if (node is IEntitySystem)
            {
                var sys = (IEntitySystem)node;
                systemUpdateEntries.Add(new SystemUpdateEntry(sys, false));
            }
        }

        private void OnChildExitingTree(Node node)
        {
            if (node is IEntity)
            {
                var ent = (IEntity)node;
                entityUpdateEntries.Add(new EntityUpdateEntry(ent, EntityUpdateType.Remove));
            }
            else if (node is IEntitySystem)
            {
                var sys = (IEntitySystem)node;
                systemUpdateEntries.Add(new SystemUpdateEntry(sys, true));
            }
        }

        private void OnExitedTree()
        {
            Disconnect("child_entered_tree", this, nameof(OnChildEnteredTree));
            Disconnect("child_exiting_tree", this, nameof(OnChildExitingTree));
            Disconnect("tree_exited", this, nameof(OnExitedTree));
        }

    }

}