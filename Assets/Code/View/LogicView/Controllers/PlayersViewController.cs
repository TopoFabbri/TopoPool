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

        private readonly Dictionary<uint, PlayerView> players = new();

        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        public void Init()
        {
            EventBus.Subscribe<PlayerCreatedEvent>(OnPlayerCreated);
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
            EventBus.Unsubscribe<PlayerCreatedEvent>(OnPlayerCreated);
            EventBus.Unsubscribe<EntityPositionUpdateEvent>(OnEntityPositionUpdate);
        }

        private void OnPlayerCreated(in PlayerCreatedEvent playerCreatedEventData)
        {
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            PlayerView instance = playerViewPrefab.Spawn(playerCreatedEventData.id, playerCreatedEventData.possess, position, rotation, transform);

            players.Add(instance.ID, instance);
        }
        
        private void OnEntityPositionUpdate(in EntityPositionUpdateEvent entityPositionUpdateEventData)
        {
            if (players.TryGetValue(entityPositionUpdateEventData.ID, out PlayerView playerView))
            {
                playerView.transform.position = new Vector3(entityPositionUpdateEventData.Position.X, entityPositionUpdateEventData.Position.Y, entityPositionUpdateEventData.Position.Z);
            }
        }
    }
}