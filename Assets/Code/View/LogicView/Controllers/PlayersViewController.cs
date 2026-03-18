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
    internal class PlayersViewController : MonoBehaviour, IInitable, ITickable, IDisposable
    {
        [SerializeField] private PlayerView playerViewPrefab;
        [SerializeField] private Transform  playersParent;

        private readonly Dictionary<uint, PlayerView> players = new();

        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        public void Init()
        {
            EventBus.Subscribe<PlayerCreatedEvent>(OnPlayerCreated);
            EventBus.Subscribe<EntityPositionUpdateEvent>(OnEntityPositionUpdate);
            EventBus.Subscribe<EntityRotationUpdateEvent>(OnEntityRotationUpdate);
            EventBus.Subscribe<EntityVelocityUpdateEvent>(OnEntityVelocityUpdate);
            EventBus.Subscribe<StickMovementIntentEvent>(OnStickIntent);
        }

        public void LateInit()
        {
        }

        public void Tick(float deltaTime)
        {
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<PlayerCreatedEvent>(OnPlayerCreated);
            EventBus.Unsubscribe<EntityPositionUpdateEvent>(OnEntityPositionUpdate);
            EventBus.Unsubscribe<EntityRotationUpdateEvent>(OnEntityRotationUpdate);
            EventBus.Unsubscribe<EntityVelocityUpdateEvent>(OnEntityVelocityUpdate);
            EventBus.Unsubscribe<StickMovementIntentEvent>(OnStickIntent);
        }

        private void OnPlayerCreated(in PlayerCreatedEvent playerCreatedEventData)
        {
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            PlayerView instance = playerViewPrefab.Spawn(playerCreatedEventData.id, playerCreatedEventData.possess, position + playersParent.position, rotation * playersParent.rotation, playersParent);

            players.Add(instance.ID, instance);
        }

        private void OnEntityPositionUpdate(in EntityPositionUpdateEvent entityPositionUpdateEventData)
        {
            if (players.TryGetValue(entityPositionUpdateEventData.ID, out PlayerView playerView))
            {
                playerView.transform.position = new Vector3(entityPositionUpdateEventData.Position.X, entityPositionUpdateEventData.Position.Y, entityPositionUpdateEventData.Position.Z);
            }
        }

        private void OnEntityRotationUpdate(in EntityRotationUpdateEvent entityRotationUpdateEventData)
        {
            if (players.TryGetValue(entityRotationUpdateEventData.ID, out PlayerView playerView))
            {
                playerView.transform.rotation = new Quaternion(entityRotationUpdateEventData.Rotation.X, entityRotationUpdateEventData.Rotation.Y, entityRotationUpdateEventData.Rotation.Z, entityRotationUpdateEventData.Rotation.W);
            }
        }

        private void OnEntityVelocityUpdate(in EntityVelocityUpdateEvent velocityData)
        {
            if (players.TryGetValue(velocityData.ID, out PlayerView playerView))
            {
                playerView.SetDesiredVelocity(new Vector3(velocityData.Velocity.X, velocityData.Velocity.Y, velocityData.Velocity.Z));
            }
        }
        
        private void OnStickIntent(in StickMovementIntentEvent stickIntentData)
        {
            if (players.TryGetValue(stickIntentData.ID, out PlayerView playerView))
            {
                playerView.UpdateStick(stickIntentData.AvgVel, stickIntentData.ForwardPos);
            }
        }
    }
}