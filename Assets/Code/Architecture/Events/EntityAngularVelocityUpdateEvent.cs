using System.Numerics;
using ImageCampus.ToolBox.Events;

namespace Architecture.Events
{
    /// <summary>
    /// Expects:
    /// <list type="number">
    /// <item><description>uint ID</description></item>
    /// <item><description>Vector3 AngularVelocity</description></item>
    /// </list>
    /// </summary>
    public struct EntityAngularVelocityUpdateEvent : IEvent
    {
        public uint    ID              { get; private set; }
        public Vector3 AngularVelocity { get; private set; }
        
        public void Assign(params object[] parameters)
        {
            ID = (uint)parameters[0];
            AngularVelocity = (Vector3)parameters[1];
        }
        
        public void Reset()
        {
            ID = 0;
            AngularVelocity = Vector3.Zero;
        }
    }
}