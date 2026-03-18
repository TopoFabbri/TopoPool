using System;
using System.Collections.Generic;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;
using UnityEngine;
using View.LogicView.EntitiesView;

namespace View.LogicView.Controllers
{
    internal sealed class BallsViewController : MonoBehaviour, IInitable, ITickable, IDisposable
    {
        [SerializeField] private BallView  whiteBallView;
        [SerializeField] private BallView  stripeBallView;
        [SerializeField] private BallView  solidBallView;
        [SerializeField] private Transform parent;

        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        private readonly Dictionary<uint, BallView> balls = new();

        public void Init()
        {
            EventBus.Subscribe<BallCreatedEvent>(OnBallCreated);
            EventBus.Subscribe<BallDestroyedEvent>(OnBallDestroyed);
            EventBus.Subscribe<EntityPositionUpdateEvent>(OnEntityPositionUpdate);
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
            EventBus.Unsubscribe<EntityPositionUpdateEvent>(OnEntityPositionUpdate);
        }

        private void OnBallCreated(in BallCreatedEvent ballCreatedData)
        {
            Vector3 position = new(ballCreatedData.position.X, ballCreatedData.position.Y, ballCreatedData.position.Z);
            Quaternion rotation = new(ballCreatedData.rotation.X, ballCreatedData.rotation.Y, ballCreatedData.rotation.Z, ballCreatedData.rotation.W);

            BallView instance;
            if (ballCreatedData.isWhite)
            {
                instance = whiteBallView.Spawn(ballCreatedData.id, false, position, rotation, parent);
            }
            else
            {
                instance = ballCreatedData.solid ? solidBallView.Spawn(ballCreatedData.id, true, position, rotation, parent) : stripeBallView.Spawn(ballCreatedData.id, false, position, rotation, parent);
            }
            
            balls.Add(instance.ID, instance);
        }

        private void OnBallDestroyed(in BallDestroyedEvent ballDestroyedData)
        {
            balls.Remove(ballDestroyedData.id, out BallView ball);

            Destroy(ball.gameObject);
        }

        private void OnEntityPositionUpdate(in EntityPositionUpdateEvent entityPositionUpdateEventData)
        {
            if (balls.TryGetValue(entityPositionUpdateEventData.ID, out BallView ball))
            {
                ball.transform.position = new Vector3(entityPositionUpdateEventData.Position.X, entityPositionUpdateEventData.Position.Y, entityPositionUpdateEventData.Position.Z);
            }
        }
    }
}