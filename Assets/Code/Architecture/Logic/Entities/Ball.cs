using System.Numerics;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic.Entities
{
    public sealed class Ball : Entity
    {
        public bool Solid { get; private set; }
     
        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        public Ball(bool solid, Vector3 position, uint id) : base(position, Quaternion.Identity, id)
        {
            Solid = solid;
        }

        public override void Init()
        {
            base.Init();
            
            EventBus.Raise<BallCreatedEvent>(Position, Rotation, Id, Solid);
        }
    }
}