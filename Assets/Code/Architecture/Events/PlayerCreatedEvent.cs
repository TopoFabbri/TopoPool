using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct PlayerCreatedEvent : IEvent
    {
        public uint id;
        public bool possess;
        
        public void Assign(params object[] parameters)
        {
            id = (uint) parameters[0];
            possess = (bool) parameters[1];
        }

        public void Reset()
        {
            id = 0;
            possess = false;
        }
    }
}