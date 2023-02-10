using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Godot;

namespace GdEcs
{

    [ExportCustomNode("KeyBezierHandle")]
    public class EntitySystemManagerNode : Node, IEntitySystemManager
    {

        public event EntityDelegate EntityAdded = delegate { };
        public event EntityDelegate EntityRemoved = delegate { };
        public event EntitySystemDelegate EntitySystemAdded = delegate { };
        public event EntitySystemDelegate EntitySystemRemoved = delegate { };

        private List<IEntity> entities = new List<IEntity>();
        private PriorityList<IEntitySystem> systems = new PriorityList<IEntitySystem>(
            (a, b) => a.EntitySystemPriority > b.EntitySystemPriority
                ? -1
                : (a.EntitySystemPriority < b.EntitySystemPriority ? 1 : 0));

        private List<SystemUpdateEntry> systemUpdateEntries = new List<SystemUpdateEntry>();
        private List<EntityUpdateEntry> entityUpdateEntries = new List<EntityUpdateEntry>();
        private Dictionary<IEntity, EmptyDelegate> entityComponentDelegates =
            new Dictionary<IEntity, EmptyDelegate>();

        public ReadOnlyCollection<IEntity> Entities => entities.AsReadOnly();

        public override void _Ready()
        {
            base._Ready();
            Connect("tree_entered", this, nameof(OnTreeEntered));
        }

        private void OnTreeEntered()
        {
            // Reset
            foreach (var system in systems.ToList())
                RemoveSystem(system);
            foreach (var entity in entities.ToList())
                RemoveEntity(entity);
            // Reconcile
            NodeUtil.TraverseChildren(this, true, (child) =>
            {
                if (child.IsEntitySystem() && NodeUtil.GetClosestParentOfType<IEntitySystemManager>(child) == this)
                {
                    AddSystem((IEntitySystem)child);
                }
                else if (child.IsEntity() && NodeUtil.GetClosestParentOfType<IEntitySystemManager>(child) == this)
                {
                    AddEntity((IEntity)child);
                }
            });
        }

        public void AddEntity(IEntity entity)
        {
            Debug.Assert(entity is Node);
            Log.Debug($"SysMgr AddEntity: {entity}");
            entityUpdateEntries.Add(new EntityUpdateEntry(entity, EntityUpdateType.Add));
        }

        public void RemoveEntity(IEntity entity)
        {
            Debug.Assert(entity is Node);
            Log.Debug($"SysMgr RemoveEntity: {entity}");
            entityUpdateEntries.Add(new EntityUpdateEntry(entity, EntityUpdateType.Remove));
        }

        public void AddSystem(IEntitySystem system)
        {
            Debug.Assert(system is Node);
            Log.Debug($"SysMgr AddSystem: {system}");
            systemUpdateEntries.Add(new SystemUpdateEntry(system, false));
        }

        public void RemoveSystem(IEntitySystem system)
        {
            Debug.Assert(system is Node);
            Log.Debug($"SysMgr RemoveSystem: {system}");
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
                        Debug.Assert(!Entities.Contains(entry.Entity));
                        entityComponentDelegates.Add(entry.Entity, () =>
                        {
                            entityUpdateEntries.Add(new EntityUpdateEntry(entry.Entity, EntityUpdateType.Update));
                        });
                        entities.Add(entry.Entity);
                        entry.Entity.GetEntityComponentStore().ComponentsChanged +=
                            entityComponentDelegates[entry.Entity];
                        EntityAdded(entry.Entity);
                        break;
                    case EntityUpdateType.Remove:
                        Debug.Assert(Entities.Contains(entry.Entity));
                        entry.Entity.GetEntityComponentStore().ComponentsChanged -=
                            entityComponentDelegates[entry.Entity];
                        entityComponentDelegates.Remove(entry.Entity);
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
                    Debug.Assert(systems.Contains(entry.System));
                    systems.Remove(entry.System);
                    EntitySystemRemoved(entry.System);
                }
                else
                {
                    Debug.Assert(!systems.Contains(entry.System));
                    systems.AddPrioritized(entry.System);
                    EntitySystemAdded(entry.System);
                    foreach (var ent in Entities)
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

    }

    internal enum EntityUpdateType
    {
        Update, Add, Remove,
    }

    internal struct EntityUpdateEntry
    {
        internal readonly IEntity Entity;
        internal readonly EntityUpdateType EntityUpdateType;

        internal EntityUpdateEntry(IEntity entity, EntityUpdateType entityUpdateType)
        {
            this.Entity = entity;
            this.EntityUpdateType = entityUpdateType;
        }
    }

    internal struct SystemUpdateEntry
    {
        internal readonly IEntitySystem System;
        internal readonly bool Remove;

        internal SystemUpdateEntry(IEntitySystem system, bool remove)
        {
            this.System = system;
            this.Remove = remove;
        }
    }

}