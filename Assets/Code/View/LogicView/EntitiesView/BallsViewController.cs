using System;
using System.Collections.Generic;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;
using UnityEngine;

namespace View.LogicView.EntitiesView
{
    internal sealed class BallsViewController : MonoBehaviour, IInitable, ITickable, IDisposable
    {
        [SerializeField] private BallView stripeBallView;
        [SerializeField] private BallView solidBallView;

        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        private readonly Dictionary<uint, BallView> balls = new();

        public void Init()
        {
            EventBus.Subscribe<BallCreatedEvent>(OnBallCreated);
            EventBus.Subscribe<BallDestroyedEvent>(OnBallDestroyed);
        }

        public void LateInit()
        {
        }

        public void Tick(float deltaTime)
        {
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<BallCreatedEvent>(OnBallCreated);
            EventBus.Unsubscribe<BallDestroyedEvent>(OnBallDestroyed);
        }

        private void OnBallCreated(in BallCreatedEvent ballCreatedData)
        {
            Vector3 position = new(ballCreatedData.position.X, ballCreatedData.position.Y, ballCreatedData.position.Z);
            Quaternion rotation = new(ballCreatedData.rotation.X, ballCreatedData.rotation.Y, ballCreatedData.rotation.Z, ballCreatedData.rotation.W);
            
            BallView instance = ballCreatedData.solid ? solidBallView.Spawn(ballCreatedData.id, true, position, rotation, transform) : stripeBallView.Spawn(ballCreatedData.id, false, position, rotation, transform);
            
            balls.Add(instance.ID, instance);
        }

        private void OnBallDestroyed(in BallDestroyedEvent ballDestroyedData)
        {
            balls.Remove(ballDestroyedData.id, out BallView ball);
            
            Destroy(ball.gameObject);
        }
    }
}