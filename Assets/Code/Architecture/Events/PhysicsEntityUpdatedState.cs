using System.Numerics;
using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    /// <summary>
    /// Expectations: [uint id, Vector3 position, Quaternion rotation].
    /// </summary>
    public struct PhysicsEntityUpdatedState : IEvent
    {
        public uint id;
        public Vector3 position;
        public Quaternion rotation;
        
        public void Assign(params object[] parameters)
        {
            id = (uint) parameters[0];
            position = (Vector3) parameters[1];
            rotation = (Quaternion) parameters[2];
        }

        public void Reset()
        {
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            id = 0;
        }
    }
}