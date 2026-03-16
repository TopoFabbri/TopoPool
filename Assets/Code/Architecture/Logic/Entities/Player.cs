using System;
using System.Numerics;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic.Entities
{
    public class Player : Entity
    {
        private float moveSpeed   = 10f;
        private float rotateSpeed = 0.1f;

        private Vector3 moveVector;
        private Vector2 rotationDelta;

        private float pitch = 0f;
        private float yaw   = 0f;

        private bool isMainPlayer;

        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        public Player(uint id) : base(id) { }

        public override void Configure(params object[] parameters)
        {
            isMainPlayer = (bool)parameters[0];
        }

        public override void Init()
        {
            base.Init();

            EventBus.Subscribe<MoveEvent>(OnMoveEvent);
            EventBus.Subscribe<RotateEvent>(OnRotateEvent);

            EventBus.Raise<PlayerCreatedEvent>(Id, isMainPlayer);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            CalculateRotation();
            
            UpdatePosition(Position + Vector3.Transform(moveVector, Rotation) * deltaTime);
        }

        public override void Dispose()
        {
            base.Dispose();

            EventBus.Unsubscribe<MoveEvent>(OnMoveEvent);
            EventBus.Unsubscribe<RotateEvent>(OnRotateEvent);
        }

        private void OnMoveEvent(in MoveEvent moveEventData)
        {
            moveVector = moveEventData.movement * moveSpeed;
        }

        private void OnRotateEvent(in RotateEvent rotateEventData)
        {
            rotationDelta = rotateEventData.rotation * rotateSpeed;
        }
        
        private void CalculateRotation()
        {
            if (rotationDelta.X == 0 && rotationDelta.Y == 0) return;

            // Accumulate yaw (horizontal) and pitch (vertical)
            yaw += rotationDelta.X;
            pitch -= rotationDelta.Y; // Subtracted to match standard non-inverted mouse look

            // Clamp vertical rotation between 90 up and 90 down
            if (pitch > 90f) pitch = 90f;
            else if (pitch < -90f) pitch = -90f;

            // Convert to radians for System.Numerics
            float yawRad   = yaw * (float)(Math.PI / 180.0);
            float pitchRad = pitch * (float)(Math.PI / 180.0);

            // CreateFromYawPitchRoll applies Yaw (World Y), Pitch (Local X), then Roll (Z)
            Quaternion newRotation = Quaternion.CreateFromYawPitchRoll(yawRad, pitchRad, 0f);
            
            UpdateRotation(newRotation);

            // Consume the delta input
            rotationDelta = Vector2.Zero;
        }
    }
}