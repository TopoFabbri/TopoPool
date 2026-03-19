using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct BallOutsideEvent : IEvent
    {
        public uint id;
        
        public void Assign(params object[] parameters)
        {
            id = (uint)parameters[0];
        }

        public void Reset()
        {
            id = 0;
        }
    }
}