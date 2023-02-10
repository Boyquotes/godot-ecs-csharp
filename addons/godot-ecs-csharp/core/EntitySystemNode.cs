using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace GdEcs
{

    public abstract class EntitySystemNode : Node, IEntitySystem
    {

        public event EntityDelegate EntityAddedToSystem = delegate { };
        public event EntityDelegate EntityRemovedFromSystem = delegate { };

        public abstract int EntitySystemPriority { get; set; }

        [Export]
        public bool SystemEnabled { get; set; } = true;

        private List<IEntity> entities = new List<IEntity>();

        protected ReadOnlyCollection<IEntity> Entities => entities.AsReadOnly();

        public void RefreshProcessesEntity(IEntity entity)
        {
            var supports = ShouldProcessEntity(entity);
            if (supports && !entities.Contains(entity))
            {
                AddEntity(entity);
            }
            else if (!supports && entities.Contains(entity))
            {
                RemoveEntity(entity);
            }
        }

        protected abstract bool ShouldProcessEntity(IEntity entity);

        protected virtual void ProcessEntity(IEntity entity, float delta) { }

        protected virtual void PhysicsProcessEntity(IEntity entity, float delta) { }

        protected void AddEntity(IEntity entity)
        {
            entities.Add(entity);
            EntityAddedToSystem(entity);
        }

        protected void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
            EntityRemovedFromSystem(entity);
        }

        public void DoProcess(float delta)
        {
            if (!SystemEnabled)
                return;
            foreach (var entity in Entities)
                ProcessEntity(entity, delta);
        }

        public void DoPhysicsProcess(float delta)
        {
            if (!SystemEnabled)
                return;
            foreach (var entity in Entities)
                PhysicsProcessEntity(entity, delta);
        }

    }

}