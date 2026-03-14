using System;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    public sealed class Scene : IInitable, ITickable, IDisposable
    {
        private BallsLogic ballsLogic;

        public Scene(int ballCount)
        {
            ballsLogic = new BallsLogic(ballCount);
        }
        
        public void Init()
        {
            ballsLogic.Init();
        }

        public void LateInit()
        {
            ballsLogic.LateInit();
        }

        public void Tick(float deltaTime)
        {
            ballsLogic.Tick(deltaTime);
        }

        public void Dispose()
        {
            ballsLogic.Dispose();
        }
    }
}