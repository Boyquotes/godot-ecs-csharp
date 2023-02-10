using System;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using Godot;
using Godot.Collections;

namespace GdEcs
{

    public sealed class EntityComponentSystem : Reference
    {

        public const ulong INVALID_ENTITY_ID = 0;

        private static readonly Lazy<EntityComponentSystem> lazyInstance =
            new Lazy<EntityComponentSystem>(() => new EntityComponentSystem());
        public static EntityComponentSystem I => lazyInstance.Value;

        private bool initialized = false;
        private ulong nextEntityId = INVALID_ENTITY_ID + 1;

        private Dictionary<IEntity, ulong> entityToEntityIdMap =
            new Dictionary<IEntity, ulong>();
        private Dictionary<IEntity, EntityComponentStore> entityToComponentStoreMap =
            new Dictionary<IEntity, EntityComponentStore>();

        private EntityComponentSystem() { }

        public void Initialize(SceneTree tree)
        {
            Debug.Assert(!initialized);
            initialized = true;

            tree.Connect("node_added", this, nameof(OnNodeAdded));
            tree.Connect("node_removed", this, nameof(OnNodeRemoved));

            tree.Root.TraverseChildren(true, OnNodeAdded);
        }

        private void OnNodeAdded(Node node)
        {
            if (node.IsEntity())
            {
                Log.Debug($"ECS IEntity added: {node.Name}");
                Debug.Assert(!entityToEntityIdMap.ContainsKey((IEntity)node),
                    $"Cannot add duplicate entity node {node} ({node.GetPath()})");
                entityToEntityIdMap.Add((IEntity)node, RequestNextEntityId());

                var sysMgr = node.GetClosestParentOfType<IEntitySystemManager>();
                Debug.Assert(sysMgr != null, $"Entity {node} ({node.GetPath()}) is not in a subtree of an IEntitySystemManager");
                sysMgr.AddEntity((IEntity)node);
            }
            else if (node.IsEntityComponent())
            {
                Log.Debug($"ECS IEntityComponent added: {node.Name}");
                var entity = node.GetClosestParentOfType<IEntity>();
                if (entity == null)
                {
                    Log.Warn($"Entity component {node} ({node.GetPath()}) added to tree, but no parent IEntity found.");
                    return;
                }
                GetEntityComponentStore(entity).AddComponentToStore((IEntityComponent)node);
            }
            else if (node.IsEntitySystem())
            {
                Log.Debug($"ECS IEntitySystem added: {node.Name}");
                var sysMgr = node.GetClosestParentOfType<IEntitySystemManager>();
                Debug.Assert(sysMgr != null, $"IEntitySystem {node} ({node.GetPath()}) is not in a subtree of an IEntitySystemManager");
                sysMgr.AddSystem((IEntitySystem)node);
            }
        }

        private void OnNodeRemoved(Node node)
        {
            if (node.IsEntity())
            {
                Log.Debug($"ECS IEntity removed: {node.Name}");
                Debug.Assert(entityToEntityIdMap.ContainsKey((IEntity)node),
                    $"Cannot remove non-existent entity node {node} ({node.GetPath()})");
                entityToEntityIdMap.Remove((IEntity)node);
                entityToComponentStoreMap.Remove((IEntity)node);

                var sysMgr = node.GetClosestParentOfType<IEntitySystemManager>();
                Debug.Assert(sysMgr != null, $"Entity {node} ({node.GetPath()}) is not in a subtree of an IEntitySystemManager");
                sysMgr.RemoveEntity((IEntity)node);
            }
            else if (node.IsEntityComponent())
            {
                Log.Debug($"ECS IEntityComponent removed: {node.Name}");
                var entity = node.GetClosestParentOfType<IEntity>();
                if (entity == null)
                {
                    Log.Warn($"Entity component {node} ({node.GetPath()}) removed from tree, but no parent IEntity found.");
                    return;
                }
                GetEntityComponentStore(entity).RemoveComponentFromStore((IEntityComponent)node);
            }
            else if (node.IsEntitySystem())
            {
                Log.Debug($"ECS IEntitySystem removed: {node.Name}");
                var sysMgr = node.GetClosestParentOfType<IEntitySystemManager>();
                Debug.Assert(sysMgr != null, $"IEntitySystem {node} ({node.GetPath()}) is not in a subtree of an IEntitySystemManager");
                sysMgr.RemoveSystem((IEntitySystem)node);
            }
        }

        public EntityComponentStore GetEntityComponentStore(IEntity entity)
        {
            if (!entityToComponentStoreMap.ContainsKey(entity))
                entityToComponentStoreMap.Add(entity, new EntityComponentStore());
            return entityToComponentStoreMap[entity];
        }

        public ulong GetEntityId(IEntity entity)
        {
            Debug.Assert(entityToEntityIdMap.ContainsKey(entity),
                $"Cannot get entity ID for non-existent (or non-entity) node {entity}");
            return entityToEntityIdMap[entity];
        }

        private ulong RequestNextEntityId()
        {
            return nextEntityId++;
        }

    }

}