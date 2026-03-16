using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct ChangeSpeedEvent : IEvent
    {
        public float Value { get; private set; }

        public void Assign(params object[] parameters)
        {
            Value = (float)parameters[0];
        }

        public void Reset()
        {
            Value = 0f;
        }
    }
}