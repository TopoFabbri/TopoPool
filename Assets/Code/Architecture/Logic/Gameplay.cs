using System;
using Architecture.Console;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    public sealed class Gameplay : IInitable, ITickable, IDisposable
    {
        private ServiceProvider ServiceProvider => ServiceProvider.Instance;
        private Time            Time            => ServiceProvider.GetService<Time>();
        private Settings        Settings        => ServiceProvider.GetService<Settings>();

        private Scene scene;

        public Gameplay(string persistentDataPath)
        {
            ServiceProvider.AddService<Time>(new Time());
            ServiceProvider.AddService<EventBus>(new EventBus());
            ServiceProvider.AddService<Settings>(new Settings(persistentDataPath));

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
            Settings.Dispose();
        }
    }
}