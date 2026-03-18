using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    /// <summary>
    /// Expects:
    /// <list type="number">
    /// <item><see cref="uint"/> ID</item>
    /// <item><see cref="float"/> ForwardPos</item>
    /// <item><see cref="float"/> AvgVel</item>
    /// </list>
    /// </summary>
    public struct StickMovementIntentEvent : IEvent
    {
        public uint  ID         { get; private set; }
        public float ForwardPos { get; private set; }
        public float AvgVel     { get; private set; }

        public void Assign(params object[] parameters)
        {
            ID = (uint)parameters[0];
            ForwardPos = (float)parameters[1];
            AvgVel = (float)parameters[2];
        }

        public void Reset()
        {
            ID = 0;
            ForwardPos = 0f;
            AvgVel = 0f;
        }
    }
}