using System.Numerics;
using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct MoveEvent : IEvent
    {
        public Vector3 movement;
        
        public void Assign(params object[] parameters)
        {
            movement = (Vector3) parameters[0];
        }

        public void Reset()
        {
            movement = Vector3.Zero;
        }
    }
}