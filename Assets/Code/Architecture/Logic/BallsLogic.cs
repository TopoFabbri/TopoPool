using System;
using System.Collections.Generic;
using System.Numerics;
using Architecture.Events;
using Architecture.Logic.Entities;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    internal sealed class BallsLogic : IInitable, ITickable, IDisposable
    {
        private readonly int ballCount;
        
        private readonly Dictionary<uint, Ball> balls = new();

        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        public BallsLogic(int ballCount)
        {
            this.ballCount = ballCount;
        }
        
        public void Init()
        {
            for (int i = 0; i < ballCount; i++)
                balls.Add((uint)i, new Ball(i >= ballCount / 2, Vector3.Zero + Vector3.UnitX * i * 0.1f + Vector3.UnitY, (uint)i));
            
            foreach (Ball ball in balls.Values)
                ball.Init();
            
            EventBus.Subscribe<BallUpdatedState>(OnBallUpdatedState);
        }

        public void LateInit()
        {
            foreach (Ball ball in balls.Values)
                ball.LateInit();
        }

        public void Tick(float deltaTime)
        {
            foreach (Ball ball in balls.Values)
                ball.Tick(deltaTime);
        }

        public void Dispose()
        {
            foreach (Ball ball in balls.Values)
                ball.Dispose();
            
            EventBus.Unsubscribe<BallUpdatedState>(OnBallUpdatedState);
        }

        private void OnBallUpdatedState(in BallUpdatedState ballUpdatedStateData)
        {
            if (balls.TryGetValue(ballUpdatedStateData.id, out Ball ball))
            {
                ball.UpdatePosition(ballUpdatedStateData.position);
                ball.UpdateRotation(ballUpdatedStateData.rotation);
            }
            else
            {
                EventBus.Raise<BallDestroyedEvent>(ballUpdatedStateData.id);
            }
        }
    }
}