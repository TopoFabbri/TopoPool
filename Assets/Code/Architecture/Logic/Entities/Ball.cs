using System.Numerics;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.Pool;
using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic.Entities
{
    public sealed class Ball : Entity
    {
        public enum Type
        {
            White,
            Solid,
            Stripe,
            Black
        }

        public Type type;
     
        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        public Ball(uint id) : base(id)
        {
        }

        public override void Configure(params object[] parameters)
        {
            UpdatePosition((Vector3) parameters[0]);
            type = (Type) parameters[1];
        }

        public override void Init()
        {
            base.Init();
            
            EventBus.Raise<BallCreatedEvent>(Position, Rotation, Id, type);
        }

        public void Reset(Vector3 position)
        {
            UpdatePosition(position);
            EventBus.Raise<EntityVelocityUpdateEvent>(Id, Vector3.Zero);
            EventBus.Raise<EntityAngularVelocityUpdateEvent>(Id, Vector3.Zero);
        }
    }
}