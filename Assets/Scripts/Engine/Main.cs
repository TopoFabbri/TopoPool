using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.Services;
using UnityEngine;

namespace Engine
{
    internal sealed class Main : MonoBehaviour
    {
        private void Awake()
        {
            ServiceProvider.Instance.AddService<EventBus>(new EventBus());
        }
    }
}
