using System;
using System.Numerics;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    public abstract class Entity : IInitable, ITickable, IDisposable
    {
        public Vector3    Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public uint Id { get; private set; }

        protected Entity(Vector3 position, Quaternion rotation, uint id)
        {
            Position = position;
            Rotation = rotation;
            Id = id;
        }
        
        public virtual void Init()
        {
        }

        public virtual void LateInit()
        {
        }

        public virtual void Tick(float deltaTime)
        {
        }

        public virtual void Dispose()
        {
        }
    }
}