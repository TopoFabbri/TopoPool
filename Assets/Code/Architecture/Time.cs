using System;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;

namespace Architecture
{
    public sealed class Time : IService, ITickable, IDisposable
    {
        public bool IsPersistant => false;

        private float lastDeltaTime;

        public Time()
        {
        }

        public void Tick(float deltaTime)
        {
            lastDeltaTime = deltaTime;
        }

        public void Dispose()
        {
        }
    }
}