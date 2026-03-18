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
        EntityFactory EntityFactory => ServiceProvider.Instance.GetService<EntityFactory>();

        public BallsLogic(int ballCount)
        {
            this.ballCount = ballCount;
        }
        
        public void Init()
        {
            Ball whiteBall = EntityFactory.CreateEntity<Ball>(new Vector3(-.3f, 0, 0), false, true);
            
            balls.Add(whiteBall.Id, whiteBall);
            
            for (int i = 0; i < ballCount; i++)
            {
                Ball instance = EntityFactory.CreateEntity<Ball>(new Vector3(i * .1f, 0, 0), i >= ballCount / 2, false);
                
                balls.Add(instance.Id, instance);
            }
            
            foreach (Ball ball in balls.Values)
                ball.Init();
            
            EventBus.Subscribe<PhysicsEntityUpdatedState>(OnBallUpdatedState);
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
            
            EventBus.Unsubscribe<PhysicsEntityUpdatedState>(OnBallUpdatedState);
        }

        private void OnBallUpdatedState(in PhysicsEntityUpdatedState physicsEntityUpdatedStateData)
        {
            if (balls.TryGetValue(physicsEntityUpdatedStateData.id, out Ball ball))
                ball.SyncPhysicsState(physicsEntityUpdatedStateData.position, physicsEntityUpdatedStateData.rotation);
        }
    }
}