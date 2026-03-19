using System.Collections;
using Architecture.Events;
using ImageCampus.ToolBox.Events;
using ImageCampus.ToolBox.ServiceProvider;
using UnityEngine;

namespace View.LogicView.EntitiesView
{
    internal sealed class BallView : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        
        public  uint ID { get; private set; }
        
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        public BallView Spawn(uint id, Vector3 position, Quaternion rotation, Transform parent)
        {
            BallView instance = Instantiate(this, position, rotation, parent);
            
            instance.ID = id;
            
            return instance;
        }
        
        public void UpdateVelocity(System.Numerics.Vector3 velocity) => rb.linearVelocity = new Vector3(velocity.X, velocity.Y, velocity.Z);
        
        public void UpdateAngularVelocity(System.Numerics.Vector3 angularVelocity) => rb.angularVelocity = new Vector3(angularVelocity.X, angularVelocity.Y, angularVelocity.Z);
        
        public void AddForce(Vector3 force, Vector3 point)
        {
            StartCoroutine(AddForceCoroutine(force, point));
        }
        
        private IEnumerator AddForceCoroutine(Vector3 force, Vector3 point, ForceMode mode = ForceMode.Impulse)
        {
            yield return new WaitForFixedUpdate();
            rb.AddForceAtPosition(force, point, mode);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Deathzone"))
                EventBus.Raise<BallOutsideEvent>(ID);
            else if (other.CompareTag("Hole"))
                EventBus.Raise<BallHoledEvent>(ID);
        }
    }
}