using Engine.Events;
using ImageCampus.ToolBox.Dataflow;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.Services;
using UnityEngine;

namespace Engine.Camera
{
    internal sealed class CamController : MonoBehaviour, IInitable, ITickable
    {
        [SerializeField] private float xSens = 10f;
        [SerializeField] private float ySens = 10f;

        [SerializeField] private float moveSpeed = 10f;

        private Vector3 moveDirection;

        public void Init()
        {
            ServiceProvider.Instance.GetService<EventBus>().Subscribe((in MoveEvent moveEvent) => moveDirection = moveEvent.direction);; 
        }
        
        public void LateInit()
        {
            
        }

        public void Tick(float deltaTime)
        {
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        }
    }
}
