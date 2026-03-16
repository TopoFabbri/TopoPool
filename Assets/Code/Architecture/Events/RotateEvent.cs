using System.Numerics;
using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct RotateEvent : IEvent
    {
        public Vector2 rotation;
        
        public void Assign(params object[] parameters)
        {
            rotation = (Vector2) parameters[0];
        }

        public void Reset()
        {
            rotation = Vector2.Zero;
        }
    }
}