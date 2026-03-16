using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using UnityEngine;

namespace View.LogicView.EntitiesView
{
    internal class PlayerView : MonoBehaviour
    {
        [SerializeField] private Camera    cam;
        [SerializeField] private Rigidbody rb;

        private System.Numerics.Vector3 position;
        private System.Numerics.Quaternion rotation;
        
        public uint ID { get; private set; }

        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        public PlayerView Spawn(uint id, bool possess, Vector3 position, Quaternion rotation, Transform parent)
        {
            PlayerView instance = Instantiate(this, position, rotation, parent);

            instance.ID = id;

            instance.cam.tag = possess ? "MainCamera" : "Untagged";
            instance.cam.enabled = possess;

            return instance;
        }

        private void FixedUpdate()
        {
            if (rb.IsSleeping())
                return;
            
            UpdatePositionAndRotation();
            EventBus.Raise<PhysicsEntityUpdatedState>(ID, position, rotation);
        }
        
        private void UpdatePositionAndRotation()
        {
            position = new System.Numerics.Vector3(transform.position.x, transform.position.y, transform.position.z);
            rotation = new System.Numerics.Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }
    }
}