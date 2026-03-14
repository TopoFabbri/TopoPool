using System;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;


namespace Architecture.Logic
{
    public sealed class Gameplay : IInitable, ITickable, IDisposable
    {
        ServiceProvider ServiceProvider => ServiceProvider.Instance;
        Time Time => ServiceProvider.GetService<Time>();
        
        public Gameplay()
        {
            ServiceProvider.AddService<Time>(new Time());
        }
            
        public void Init()
        {
        }

        public void LateInit()
        {
        }

        public void Tick(float deltaTime)
        {
            Time.Tick(deltaTime);
        }

        public void Dispose()
        {
        }
    }
}