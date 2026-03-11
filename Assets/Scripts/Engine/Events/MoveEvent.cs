using ImageCampus.ToolBox.Events;
using UnityEngine;

namespace Engine.Events
{
    internal struct MoveEvent : IEvent
    {
        internal Vector3 direction;
        
        public void Assign(params object[] parameters)
        {
            direction = (Vector3) parameters[0];
        }

        public void Reset()
        {
            direction = Vector3.zero;
        }
    }
}