using System.Numerics;
using Architecture.Logic.Entities;
using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct BallCreatedEvent : IEvent
    {
        public Vector3 position;
        public Quaternion rotation;
        public uint id;
        public Ball.Type type;
        
        public void Assign(params object[] parameters)
        {
            position = (Vector3)parameters[0];
            rotation = (Quaternion)parameters[1];
            id = (uint)parameters[2];
            type = (Ball.Type)parameters[3];
        }

        public void Reset()
        {
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            id = 0;
            type = Ball.Type.White;
        }
    }
}