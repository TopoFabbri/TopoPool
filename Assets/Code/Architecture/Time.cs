using System;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;

namespace Architecture
{
    public sealed class Time : IService, ITickable, IDisposable
    {
        private float lastDeltaTime;

        public bool IsPersistant => false;

        public float Delta { get; private set; }

        public void Tick(float deltaTime)
        {
            lastDeltaTime = deltaTime;
            Delta = deltaTime;
        }

        public void Dispose()
        {
        }
    }
}