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
        private readonly IPoolRack rack;

        private readonly Dictionary<uint, Ball> balls = new();
        private readonly Dictionary<uint, Vector3> ballPositions = new();

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
                ballPositions.Add(whiteBall.Id, whiteBall.Position);
            }

            foreach (Vector3 pos in rack.BlackRack)
            {
                Ball blackBall = EntityFactory.CreateEntity<Ball>(pos, Ball.Type.Black);
                balls.Add(blackBall.Id, blackBall);
                ballPositions.Add(blackBall.Id, blackBall.Position);
            }

            foreach (Vector3 pos in rack.SolidRack)
            {
                Ball solidBall = EntityFactory.CreateEntity<Ball>(pos, Ball.Type.Solid);
                balls.Add(solidBall.Id, solidBall);
                ballPositions.Add(solidBall.Id, solidBall.Position);
            }

            foreach (Vector3 pos in rack.StripeRack)
            {
                Ball stripeBall = EntityFactory.CreateEntity<Ball>(pos, Ball.Type.Stripe);
                balls.Add(stripeBall.Id, stripeBall);
                ballPositions.Add(stripeBall.Id, stripeBall.Position);
            }

            foreach (Ball ball in balls.Values)
                ball.Init();

            EventBus.Subscribe<PhysicsEntityUpdatedState>(OnBallUpdatedState);
            EventBus.Subscribe<BallOutsideEvent>(OnBallOutside);
            EventBus.Subscribe<BallHoledEvent>(OnBallHoled);
        }

        private void OnBallHoled(in BallHoledEvent ballHoledEventData)
        {
            if (balls.TryGetValue(ballHoledEventData.ID, out Ball ball))
            {
                switch (ball.type)
                {
                    case Ball.Type.White:
                        ball.Reset(ballPositions[ballHoledEventData.ID]);
                        break;
                    
                    case Ball.Type.Solid:
                    case Ball.Type.Stripe:
                    case Ball.Type.Black:
                        ballPositions.Remove(ball.Id);
                        balls.Remove(ball.Id);
                        ball.Dispose();
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                    
            }
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
            EventBus.Unsubscribe<BallOutsideEvent>(OnBallOutside);
        }

        private void OnBallUpdatedState(in PhysicsEntityUpdatedState physicsEntityUpdatedStateData)
        {
            if (balls.TryGetValue(physicsEntityUpdatedStateData.id, out Ball ball))
                ball.SyncPhysicsState(physicsEntityUpdatedStateData.position, physicsEntityUpdatedStateData.rotation);
        }

        private void OnBallOutside(in BallOutsideEvent ballOutsideData)
        {
            if (!balls.TryGetValue(ballOutsideData.id, out Ball ball)) return;
            
            ball.Reset(ballPositions[ballOutsideData.id]);
        }
    }
}