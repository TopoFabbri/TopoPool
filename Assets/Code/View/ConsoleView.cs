using System;
using Architecture.Console.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;
using UnityEngine;

namespace View
{
    internal sealed class ConsoleView : MonoBehaviour, IInitable, IDisposable
    {
        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        public void Init()
        {
            EventBus.Subscribe<ConsoleLogEvent>(Log);
            EventBus.Subscribe<ConsoleWarningEvent>(LogWarning);
            EventBus.Subscribe<ConsoleErrorEvent>(LogError);
        }

        public void LateInit()
        {
        }

        public void Dispose()
        {
        }

        private void Log(in ConsoleLogEvent callback)
        {
            Debug.Log(callback.message);
        }

        private void LogWarning(in ConsoleWarningEvent callback)
        {
            Debug.LogWarning(callback.message);
        }

        private void LogError(in ConsoleErrorEvent callback)
        {
            Debug.LogError(callback.message);
        }
    }
}