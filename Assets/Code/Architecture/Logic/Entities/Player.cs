using System;
using System.Collections.Generic;
using System.Numerics;
using Architecture.Events;
using Architecture.Logic.Entities.PlayerStates;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.StateMachine;

namespace Architecture.Logic.Entities
{
    public class Player : Entity
    {
        private const int MAX_VELOCITY_SAMPLES = 4;
            
        private       float moveSpeed            = 10f;

        private Vector3 moveVector;
        private Vector2 rotationDelta;
        private Queue<float> stickVelHistory = new(MAX_VELOCITY_SAMPLES);
        
        private float   stickPos;
        private Vector2 rotation;

        private bool isMainPlayer;

        private FSM<PlayerStates.PlayerStates, PlayerFlags> fsm;

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
            EventBus.Subscribe<ChangeSpeedEvent>(OnChangeSpeedEvent);

            EventBus.Raise<PlayerCreatedEvent>(Id, isMainPlayer);

            InitFsm();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            fsm.Tick();
            
            rotationDelta = Vector2.Zero;
        }

        public override void Dispose()
        {
            base.Dispose();

            EventBus.Unsubscribe<MoveEvent>(OnMoveEvent);
            EventBus.Unsubscribe<RotateEvent>(OnRotateEvent);
            EventBus.Unsubscribe<ChangeSpeedEvent>(OnChangeSpeedEvent);
        }

        private void InitFsm()
        {
            fsm = new FSM<PlayerStates.PlayerStates, PlayerFlags>(PlayerStates.PlayerStates.FreeCam);
            
            fsm.AddState<PlayerFreeCamState>(PlayerStates.PlayerStates.FreeCam, onTickParameters: () => new object[]
            {
                (Action<Vector2, Quaternion>)SetRotation,
                (Action<Vector3>)RaiseVelocityUpdateEvent,
                rotation,
                rotationDelta,
                moveVector,
                moveSpeed
            });

            fsm.AddState<PlayerStickModeState>(PlayerStates.PlayerStates.StickMode, onTickParameters: () => new object[]
            {
                (Action<float, float>)RaiseStickMovementEvent,
                rotationDelta,
                stickPos,
                stickVelHistory
            });
            
            fsm.SetTransition(PlayerStates.PlayerStates.FreeCam, PlayerFlags.EnterStickMode, PlayerStates.PlayerStates.StickMode);
            
            fsm.SetTransition(PlayerStates.PlayerStates.StickMode, PlayerFlags.EnterFreeCam, PlayerStates.PlayerStates.FreeCam);
        }

        private void OnMoveEvent(in MoveEvent moveEventData)
        {
            moveVector = moveEventData.movement;
        }

        private void OnRotateEvent(in RotateEvent rotateEventData)
        {
            rotationDelta += rotateEventData.rotation;
        }

        private void OnChangeSpeedEvent(in ChangeSpeedEvent changeSpeedEventData)
        {
            moveSpeed += changeSpeedEventData.Value;

            moveSpeed = Math.Clamp(moveSpeed, Settings.MinSpeed, Settings.MaxSpeed);
        }
        
        private void RaiseStickMovementEvent(float pos, float avgVel)
        {
            stickPos = pos;
            EventBus.Raise<StickMovementIntentEvent>(Id, pos, avgVel);
        }
        
        private void RaiseVelocityUpdateEvent(Vector3 vel) => EventBus.Raise<EntityVelocityUpdateEvent>(Id, vel);

        private void SetRotation(Vector2 rotation, Quaternion quatRotation)
        {
            this.rotation = rotation;
            UpdateRotation(quatRotation);
        }
    }
}