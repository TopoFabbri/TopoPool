using System;
using Architecture.Console;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    public sealed class Gameplay : IInitable, ITickable, IDisposable
    {
        ServiceProvider ServiceProvider => ServiceProvider.Instance;
        Time Time => ServiceProvider.GetService<Time>();

        private Scene scene;
        
        public Gameplay()
        {
            ServiceProvider.AddService<Time>(new Time());
            ServiceProvider.AddService<EventBus>(new EventBus());
            ServiceProvider.AddService<Settings>(new Settings());
            
            scene = new Scene(10);
        }
            
        public void Init()
        {
            scene.Init();
            
            GameConsole.Log("Gameplay initialized");
        }

        public void LateInit()
        {
            scene.LateInit();

            GameConsole.Log("Gameplay late initialized");
        }

        public void Tick(float deltaTime)
        {
            Time.Tick(deltaTime);
            scene.Tick(deltaTime);
        }

        public void Dispose()
        {
            scene.Dispose();
        }
    }
}