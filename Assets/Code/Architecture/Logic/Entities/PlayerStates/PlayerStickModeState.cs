using System;
using System.Collections.Generic;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.StateMachine;

namespace Architecture.Logic.Entities.PlayerStates
{
    public class PlayerStickModeState : State
    {
        public override Type[] OnTickParamTypes =>
            new[]
            {
                typeof(Action<float, float>),
                typeof(System.Numerics.Vector2),
                typeof(float),
                typeof(Queue<float>)
            };

        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private Time     Time     => ServiceProvider.Instance.GetService<Time>();
        private Settings Settings => ServiceProvider.Instance.GetService<Settings>();

        public override BehaviourActions GetOnEnterBehaviours(params object[] parameters)
        {
            BehaviourActions actions = Pool.Get<BehaviourActions>();

            actions.AddMultiThreadableBehaviour(0, () => EventBus.Subscribe<ToggleStickModeEvent>(OnToggleStickModeEvent));

            return actions;
        }

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            Action<float, float> stickMovementSetter = (Action<float, float>)parameters[0];
            System.Numerics.Vector2 rotDelta = (System.Numerics.Vector2)parameters[1];
            float curPos = (float)parameters[2];
            Queue<float> velocityHistory = (Queue<float>)parameters[3];

            BehaviourActions actions = Pool.Get<BehaviourActions>();

            actions.AddMainThreadBehaviour(0, () =>
            {
                CalculateNewPos(out float newPos, rotDelta.Y, curPos);
                CalculateInstantVelocity(out float instantVelocity, newPos, curPos);
                UpdateVelocityHistory(velocityHistory, instantVelocity);
                CalculateAverageVelocity(out float averageVelocity, velocityHistory);

                stickMovementSetter(newPos, averageVelocity);
            });

            return actions;
        }

        public override BehaviourActions GetOnExitBehaviour(params object[] parameters)
        {
            BehaviourActions actions = Pool.Get<BehaviourActions>();

            actions.AddMultiThreadableBehaviour(0, () => EventBus.Unsubscribe<ToggleStickModeEvent>(OnToggleStickModeEvent));

            return actions;
        }

        private void CalculateNewPos(out float newPos, float rotDelta, float curPos)
        {
            float moveDelta = rotDelta * Settings.StickSensitivity * Time.Delta;
            newPos = curPos + moveDelta;

            newPos = Math.Clamp(newPos, Settings.StickRangeMin, Settings.StickRangeMax);
        }

        private void CalculateInstantVelocity(out float instantVelocity, float newPos, float curPos)
        {
            instantVelocity = (newPos - curPos) / Time.Delta;
        }

        private static void UpdateVelocityHistory(Queue<float> velocityHistory, float instantVelocity)
        {
            velocityHistory.Enqueue(instantVelocity);

            if (velocityHistory.Count > 4)
                velocityHistory.Dequeue();
        }

        private static void CalculateAverageVelocity(out float averageVelocity, Queue<float> velocityHistory)
        {
            float sum = 0f;
                
            foreach (float vel in velocityHistory)
                sum += vel;
                
            averageVelocity = sum / velocityHistory.Count;
        }

        private void OnToggleStickModeEvent(in ToggleStickModeEvent toggleStickModeEventData)
        {
            if (!toggleStickModeEventData.IsEnteringStickMode)
                flag.Invoke(PlayerFlags.EnterFreeCam);
        }
    }
}