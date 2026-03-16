using System;
using Architecture.Logic.Entities;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    public sealed class Scene : IInitable, ITickable, IDisposable
    {
        private BallsLogic ballsLogic;

        private Player player;
        
        EntityFactory EntityFactory => ServiceProvider.Instance.GetService<EntityFactory>();
        
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
        }
    }
}