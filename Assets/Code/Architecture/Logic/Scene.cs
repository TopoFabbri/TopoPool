using System;
using Architecture.Events;
using Architecture.Logic.Entities;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    public sealed class Scene : IInitable, ITickable, IDisposable
    {
        private BallsLogic ballsLogic;

        private Player player;

        private EntityFactory EntityFactory => ServiceProvider.Instance.GetService<EntityFactory>();
        private EventBus      EventBus      => ServiceProvider.Instance.GetService<EventBus>();

        public Scene(int ballCount)
        {
            ServiceProvider.Instance.AddService<EntityFactory>(new EntityFactory());

            ballsLogic = new BallsLogic(ballCount);
        }

        public void Init()
        {
            player = EntityFactory.CreateEntity<Player>(true);

            player.Init();
            ballsLogic.Init();

            EventBus.Subscribe<PhysicsEntityUpdatedState>(OnPhysicsUpdated);
        }

        public void LateInit()
        {
            player.LateInit();
            ballsLogic.LateInit();
        }

        public void Tick(float deltaTime)
        {
            player.Tick(deltaTime);
            ballsLogic.Tick(deltaTime);
        }

        public void Dispose()
        {
            player.Dispose();
            EntityFactory.Dispose();
            ballsLogic.Dispose();

            EventBus.Unsubscribe<PhysicsEntityUpdatedState>(OnPhysicsUpdated);
        }

        private void OnPhysicsUpdated(in PhysicsEntityUpdatedState physicsEntityUpdatedStateData)
        {
            if (physicsEntityUpdatedStateData.id == player.Id)
                player.SyncPhysicsState(physicsEntityUpdatedStateData.position, physicsEntityUpdatedStateData.rotation);
        }
    }
}