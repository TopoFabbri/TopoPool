using Engine.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Engine.Input
{
    internal sealed class InputListener : MonoBehaviour
    {
        public void OnMove(InputValue input)
        {
            ServiceProvider.Instance.GetService<EventBus>().Raise<MoveEvent>(input.Get<Vector2>());
        }
    }
}
