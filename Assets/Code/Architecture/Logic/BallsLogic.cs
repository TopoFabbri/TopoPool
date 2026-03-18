using System;
using System.Collections.Generic;
using System.Numerics;
using Architecture.Events;
using Architecture.Logic.Entities;
using Architecture.Logic.Entities.BallRacks;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    internal sealed class BallsLogic : IInitable, ITickable, IDisposable
    {
        IPoolRack rack;

        private readonly Dictionary<uint, Ball> balls = new();

        EventBus      EventBus      => ServiceProvider.Instance.GetService<EventBus>();
        EntityFactory EntityFactory => ServiceProvider.Instance.GetService<EntityFactory>();

        public BallsLogic(IPoolRack rack)
        {
            this.rack = rack;
            
            if (rack == null)
                this.rack = new DefaultRack();
        }

        public void Init()
        {
            foreach (Vector3 pos in rack.WhiteRack)
            {
                Ball whiteBall = EntityFactory.CreateEntity<Ball>(pos, Ball.Type.White);
                balls.Add(whiteBall.Id, whiteBall);
            }

            foreach (Vector3 pos in rack.BlackRack)
            {
                Ball whiteBall = EntityFactory.CreateEntity<Ball>(pos, Ball.Type.Black);
                balls.Add(whiteBall.Id, whiteBall);
            }

            foreach (Vector3 pos in rack.SolidRack)
            {
                Ball solidBall = EntityFactory.CreateEntity<Ball>(pos, Ball.Type.Solid);
                balls.Add(solidBall.Id, solidBall);
            }

            foreach (Vector3 pos in rack.StripeRack)
            {
                Ball stripeBall = EntityFactory.CreateEntity<Ball>(pos, Ball.Type.Stripe);
                balls.Add(stripeBall.Id, stripeBall);
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