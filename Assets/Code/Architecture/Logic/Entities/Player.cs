using System.Numerics;
using Architecture.Console;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic.Entities
{
    public class Player : Entity
    {
        private float   moveSpeed = 10f;
        private Vector3 moveVector;

        private bool isMainPlayer;

        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        public Player(uint id) : base(id)
        {
        }

        public override void Configure(params object[] parameters)
        {
            isMainPlayer = (bool) parameters[0];
        }

        public override void Init()
        {
            base.Init();
            
            EventBus.Subscribe<MoveEvent>(OnMoveEvent);
            EventBus.Raise<PlayerCreatedEvent>(Id, isMainPlayer);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            
            UpdatePosition(Position + moveVector * deltaTime);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            EventBus.Unsubscribe<MoveEvent>(OnMoveEvent);
        }

        private void OnMoveEvent(in MoveEvent moveEventData)
        {
            moveVector = moveEventData.movement * moveSpeed;
        }
    }
}