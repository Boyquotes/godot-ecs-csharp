using Godot;

namespace GdEcs
{

    public delegate void EntityDelegate(IEntity entity);
    public delegate void NodeDelegate(Node node);

    public interface IEntityComponent { }

    public interface IEntity
    {
        ulong EntityId { get; set; }

        EntityComponentStore ComponentStore { get; }
    }

    public interface IEntitySystem
    {
        void RefreshProcessesEntity(IEntity entity);

        void DoProcess(float delta);
        void DoPhysicsProcess(float delta);
    }

}