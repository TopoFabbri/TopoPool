using System.Numerics;
using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct BallCreatedEvent : IEvent
    {
        public Vector3 position;
        public Quaternion rotation;
        public uint id;
        public bool solid;
        public bool isWhite;
        
        public void Assign(params object[] parameters)
        {
            position = (Vector3)parameters[0];
            rotation = (Quaternion)parameters[1];
            id = (uint)parameters[2];
            solid = (bool)parameters[3];
            isWhite = (bool)parameters[4];
        }

        public void Reset()
        {
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            id = 0;
            solid = false;
            isWhite = false;
        }
    }
}