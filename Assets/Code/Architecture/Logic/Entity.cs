using System;
using System.Numerics;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    public abstract class Entity : IInitable, ITickable, IDisposable
    {
        protected Vector3    Position { get; private set; }
        protected Quaternion Rotation { get; private set; }

        public uint Id { get; private set; }

        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        protected Entity(uint id)
        {
            Id = id;
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
        }

        public abstract void Configure(params object[] parameters);

        public virtual void Init()
        {
        }

        public virtual void LateInit()
        {
        }

        public virtual void Tick(float deltaTime)
        {
        }

        public void SyncPhysicsState(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public void UpdatePosition(Vector3 position)
        {
            Position = position;
            EventBus.Raise<EntityPositionUpdateEvent>(Id, position);
        }

        public void UpdateRotation(Quaternion rotation)
        {
            Rotation = rotation;
            EventBus.Raise<EntityRotationUpdateEvent>(Id, rotation);
        }

        public virtual void Dispose()
        {
        }
    }
}