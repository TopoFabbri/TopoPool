using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct EntityDestroyedEvent : IEvent
    {
        public uint ID { get; private set; }
        
        public void Assign(params object[] parameters)
        {
            ID = (uint)parameters[0];
        }
        
        public void Reset()
        {
            ID = 0;
        }
    }
}