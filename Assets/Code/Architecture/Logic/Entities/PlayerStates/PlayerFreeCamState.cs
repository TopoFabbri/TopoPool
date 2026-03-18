using System;
using System.Numerics;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.StateMachine;

namespace Architecture.Logic.Entities.PlayerStates
{
    /// <summary>
    /// State handling free camera movement and rotation.
    /// <para>Expects 7 parameters in <see cref="GetOnTickBehaviours"/>:</para>
    /// <list type="number">
    /// <item><description><see cref="Action{Vector2, Quaternion}"/> rotationSetter: Callback to apply the calculated rotation.</description></item>
    /// <item><description><see cref="Action{Vector3}"/> positionSetter: Callback to apply the calculated position delta.</description></item>
    /// <item><description><see cref="Vector2"/> rotation: The current Euler angles (X: Yaw, Y: Pitch).</description></item>
    /// <item><description><see cref="Vector2"/> rotDelta: The input delta for rotation.</description></item>
    /// <item><description><see cref="Vector3"/> moveDir: The local movement direction vector.</description></item>
    /// <item><description><see cref="float"/> moveSpeed: Movement speed multiplier.</description></item>
    /// </list>
    /// </summary>
    public class PlayerFreeCamState : State
    {
        public override Type[] OnTickParamTypes =>
            new[]
            {
                typeof(Action<Vector2, Quaternion>),
                typeof(Action<Vector3>),
                typeof(Vector2),
                typeof(Vector2),
                typeof(Vector3),
                typeof(float),
            };

        private Settings Settings => ServiceProvider.Instance.GetService<Settings>();
        private Time Time => ServiceProvider.Instance.GetService<Time>();
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        public override BehaviourActions GetOnEnterBehaviours(params object[] parameters)
        {
            BehaviourActions actions = Pool.Get<BehaviourActions>();
            
            actions.AddMultiThreadableBehaviour(0, () => EventBus.Subscribe<ToggleStickModeEvent>(OnToggleStickModeEvent));
            
            return actions;
        }

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            Action<Vector2, Quaternion> rotationSetter = (Action<Vector2, Quaternion>)parameters[0];
            Action<Vector3> velocitySetter = (Action<Vector3>)parameters[1];
            Vector2 rotation = (Vector2)parameters[2];
            Vector2 rotDelta = (Vector2)parameters[3];
            Vector3 moveDir = (Vector3)parameters[4];
            float moveSpeed = (float)parameters[5];

            Quaternion newRot = Quaternion.Identity;
            Vector3 newVel = Vector3.Zero;

            BehaviourActions actions = Pool.Get<BehaviourActions>();

            actions.AddMultiThreadableBehaviour(0, () => newRot = CalculateRotation(ref rotation, rotDelta));
            actions.AddMultiThreadableBehaviour(1, () => newVel = CalculateVelocity(moveDir, newRot, moveSpeed));
            actions.AddMainThreadBehaviour(2, () => rotationSetter(rotation, newRot));
            actions.AddMainThreadBehaviour(3, () => velocitySetter(newVel));

            return actions;
        }

        public override BehaviourActions GetOnExitBehaviour(params object[] parameters)
        {
            BehaviourActions actions = Pool.Get<BehaviourActions>();
            
            actions.AddMultiThreadableBehaviour(0, () => EventBus.Unsubscribe<ToggleStickModeEvent>(OnToggleStickModeEvent));
            
            return actions;
        }

        private Quaternion CalculateRotation(ref Vector2 rotation, Vector2 rotDelta)
        {
            if (rotDelta.Length() == 0)
                return CreateRotation(rotation);

            rotation.X += rotDelta.X * Settings.HorizontalSensitivity * Time.Delta;
            rotation.Y -= rotDelta.Y * Settings.VerticalSensitivity * Time.Delta;

            rotation.Y = Math.Clamp(rotation.Y, Settings.MinVerticalAngle, Settings.MaxVerticalAngle);

            return CreateRotation(rotation);
        }

        private Vector3 CalculateVelocity(Vector3 moveDir, Quaternion rotation, float moveSpeed)
        {
            moveDir *= moveSpeed;

            return Vector3.Transform(moveDir, rotation);
        }

        private Quaternion CreateRotation(Vector2 rotation) => Quaternion.CreateFromYawPitchRoll(rotation.X * (float)(Math.PI / 180.0), rotation.Y * (float)(Math.PI / 180.0), 0f);
        
        private void OnToggleStickModeEvent(in ToggleStickModeEvent toggleStickModeEventData)
        {
            if (toggleStickModeEventData.IsEnteringStickMode)
                flag.Invoke(PlayerFlags.EnterStickMode);
        }
    }
}