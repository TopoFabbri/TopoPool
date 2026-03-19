using System;
using System.Collections.Generic;
using Architecture.Events;
using Architecture.Logic.Entities;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;
using Sirenix.OdinInspector;
using UnityEngine;
using View.LogicView.EntitiesView;

namespace View.LogicView.Controllers
{
    internal sealed class BallsViewController : SerializedMonoBehaviour, IInitable, ITickable, IDisposable
    {
        [SerializeField] private Transform parent;

        [SerializeField] private Dictionary<Ball.Type, BallView> ballViewPrefabs;
        
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        private readonly Dictionary<uint, BallView> balls = new();

        public void Init()
        {
            EventBus.Subscribe<BallCreatedEvent>(OnBallCreated);
            EventBus.Subscribe<EntityPositionUpdateEvent>(OnEntityPositionUpdate);
            EventBus.Subscribe<EntityVelocityUpdateEvent>(OnEntityVelocityUpdate);
            EventBus.Subscribe<EntityAngularVelocityUpdateEvent>(OnEntityVelocityUpdate);
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
            EventBus.Unsubscribe<EntityPositionUpdateEvent>(OnEntityPositionUpdate);
            EventBus.Unsubscribe<EntityVelocityUpdateEvent>(OnEntityVelocityUpdate);
            EventBus.Unsubscribe<EntityAngularVelocityUpdateEvent>(OnEntityVelocityUpdate);
        }

        private void OnBallCreated(in BallCreatedEvent ballCreatedData)
        {
            Vector3 position = new(ballCreatedData.position.X, ballCreatedData.position.Y, ballCreatedData.position.Z);
            Quaternion rotation = new(ballCreatedData.rotation.X, ballCreatedData.rotation.Y, ballCreatedData.rotation.Z, ballCreatedData.rotation.W);

            BallView instance = ballViewPrefabs[ballCreatedData.type].Spawn(ballCreatedData.id, position, rotation, parent);
            
            balls.Add(instance.ID, instance);
        }

        private void OnEntityPositionUpdate(in EntityPositionUpdateEvent entityPositionUpdateEventData)
        {
            if (balls.TryGetValue(entityPositionUpdateEventData.ID, out BallView ball))
            {
                ball.transform.position = new Vector3(entityPositionUpdateEventData.Position.X, entityPositionUpdateEventData.Position.Y, entityPositionUpdateEventData.Position.Z);
            }
        }

        private void OnEntityVelocityUpdate(in EntityVelocityUpdateEvent entityVelocityUpdateEventData)
        {
            if (balls.TryGetValue(entityVelocityUpdateEventData.ID, out BallView ball))
            {
                ball.UpdateVelocity(entityVelocityUpdateEventData.Velocity);
            }
        }

        private void OnEntityVelocityUpdate(in EntityAngularVelocityUpdateEvent angularVelocityUpdateEventData)
        {
            if (balls.TryGetValue(angularVelocityUpdateEventData.ID, out BallView ball))
            {
                ball.UpdateAngularVelocity(angularVelocityUpdateEventData.AngularVelocity);
            }
        }
    }
}