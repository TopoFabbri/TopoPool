using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.StateMachine;

namespace Architecture.Logic.Entities.PlayerStates
{
    public class PlayerStickModeState : State
    {
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        public override BehaviourActions GetOnEnterBehaviours(params object[] parameters)
        {
            BehaviourActions actions = Pool.Get<BehaviourActions>();
            
            actions.AddMultiThreadableBehaviour(0, () => EventBus.Subscribe<ToggleStickModeEvent>(OnToggleStickModeEvent));
            
            return actions;
        }

        public override BehaviourActions GetOnExitBehaviour(params object[] parameters)
        {
            BehaviourActions actions = Pool.Get<BehaviourActions>();
            
            actions.AddMultiThreadableBehaviour(0, () => EventBus.Unsubscribe<ToggleStickModeEvent>(OnToggleStickModeEvent));
            
            return actions;
        }
        
        private void OnToggleStickModeEvent(in ToggleStickModeEvent toggleStickModeEventData)
        {
            if (!toggleStickModeEventData.IsEnteringStickMode)
                flag.Invoke(PlayerFlags.EnterFreeCam);
        }
    }
}