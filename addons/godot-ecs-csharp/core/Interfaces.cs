namespace GdEcs
{

    public delegate void EntityDelegate(IEntity entity);

    public interface IEntityComponent { }

    public interface IEntity
    {
        EntityComponentStore ComponentStore { get; }
    }

    public interface IEntitySystem
    {
        void RefreshProcessesEntity(IEntity entity);

        void DoProcess(float delta);
        void DoPhysicsProcess(float delta);
    }

}