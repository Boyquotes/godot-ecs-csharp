using Godot;

namespace GdEcs
{

    public delegate void EmptyDelegate();
    public delegate void EntityDelegate(IEntity entity);
    public delegate void EntitySystemDelegate(IEntitySystem system);
    public delegate void NodeDelegate(Node node);

    public interface IEntityComponent { }

    public interface IEntity { }

    public interface IEntitySystemManager
    {
        void AddSystem(IEntitySystem system);
        void RemoveSystem(IEntitySystem system);
        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);
    }

    public interface IEntitySystem
    {
        int EntitySystemPriority { get; }

        void RefreshProcessesEntity(IEntity entity);

        void DoProcess(float delta);
        void DoPhysicsProcess(float delta);
    }

}