using System.Numerics;
using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct EntityRotationUpdateEvent : IEvent
    {
        public uint       ID       { get; private set; }
        public Quaternion Rotation { get; private set; }
        
        public void Assign(params object[] parameters)
        {
            ID = (uint) parameters[0];
            Rotation = (Quaternion) parameters[1];
        }

        public void Reset()
        {
            ID = 0;
            Rotation = Quaternion.Identity;
        }
    }
}