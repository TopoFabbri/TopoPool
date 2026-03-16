using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = System.Numerics.Vector3;

namespace View.LogicView
{
    public class InputListener : MonoBehaviour
    {
        EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        public void OnMove(InputValue input)
        {
            UnityEngine.Vector3 movement = input.Get<UnityEngine.Vector3>();

            EventBus.Raise<MoveEvent>(new Vector3(movement.x, movement.y, movement.z));
        }
    }
}