using System.Numerics;
using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct EntityVelocityUpdateEvent : IEvent
    {
        public uint    ID       { get; private set; }
        public Vector3 Velocity { get; private set; }

        public void Assign(params object[] parameters)
        {
            ID = (uint)parameters[0];
            Velocity = (Vector3)parameters[1];
        }

        public void Reset()
        {
            ID = 0;
            Velocity = Vector3.Zero;
        }
    }
}