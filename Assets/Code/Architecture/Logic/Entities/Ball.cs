using System.Numerics;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic.Entities
{
    public sealed class Ball : Entity
    {
        public bool Solid { get; private set; }
        public bool IsWhite { get; private set;}
     
        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        public Ball(uint id) : base(id)
        {
        }

        public override void Configure(params object[] parameters)
        {
            UpdatePosition((Vector3) parameters[0]);
            Solid = (bool) parameters[1];
            IsWhite = (bool) parameters[2];
        }

        public override void Init()
        {
            base.Init();
            
            EventBus.Raise<BallCreatedEvent>(Position, Rotation, Id, Solid, IsWhite);
        }
    }
}