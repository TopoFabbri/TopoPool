using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct ToggleStickModeEvent : IEvent
    {
        public bool IsEnteringStickMode { get; private set; }

        public void Assign(params object[] parameters)
        {
            IsEnteringStickMode = (bool)parameters[0];
        }

        public void Reset()
        {
            IsEnteringStickMode = false;
        }
    }
}