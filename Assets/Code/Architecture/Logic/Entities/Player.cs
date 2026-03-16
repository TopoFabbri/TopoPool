using System;
using System.Numerics;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic.Entities
{
    public class Player : Entity
    {
        private float moveSpeed = 10f;

        private Vector3 moveVector;
        private Vector2 rotationDelta;

        private float pitch = 0f;
        private float yaw   = 0f;

        private bool isMainPlayer;

        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        Settings Settings => ServiceProvider.Instance.GetService<Settings>();

        public Player(uint id) : base(id)
        {
        }

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
            rotationDelta.X = rotateEventData.rotation.X * Settings.HorizontalSensitivity;
            rotationDelta.Y = rotateEventData.rotation.Y * Settings.VerticalSensitivity;
        }

        private void CalculateRotation()
        {
            if (rotationDelta is { X: 0, Y: 0 }) return;

            yaw += rotationDelta.X;
            pitch -= rotationDelta.Y;

            if (pitch > 90f) pitch = 90f;
            else if (pitch < -90f) pitch = -90f;

            Quaternion newRotation = Quaternion.CreateFromYawPitchRoll(yaw * (float)(Math.PI / 180.0), pitch * (float)(Math.PI / 180.0), 0f);

            UpdateRotation(newRotation);

            rotationDelta = Vector2.Zero;
        }
    }
}