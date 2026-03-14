using System.Numerics;
using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    public struct BallUpdatedState : IEvent
    {
        public Vector3 position;
        public Quaternion rotation;
        public uint id;
        
        public void Assign(params object[] parameters)
        {
            position = (Vector3) parameters[0];
            rotation = (Quaternion) parameters[1];
            id = (uint) parameters[2];
        }

        public void Reset()
        {
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            id = 0;
        }
    }
}