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
        [SerializeField] private Transform stick;
        [SerializeField] private Transform stickPoint;
        [SerializeField] private LayerMask sweepMask;
        
        private System.Numerics.Vector3    position;
        private System.Numerics.Quaternion rotation;
        private Vector3                    desiredVelocity;
        private float                      avgStickVel;
        private float                      stickPos;

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
            if (rb.IsSleeping() && desiredVelocity.sqrMagnitude == 0)
                return;

            rb.linearVelocity = new Vector3(desiredVelocity.x, desiredVelocity.y, desiredVelocity.z);

            UpdatePositionAndRotation();
            EventBus.Raise<PhysicsEntityUpdatedState>(ID, position, rotation);
        }

        private void UpdatePositionAndRotation()
        {
            position = new System.Numerics.Vector3(transform.position.x, transform.position.y, transform.position.z);
            rotation = new System.Numerics.Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }

        public void SetDesiredVelocity(Vector3 velocity)
        {
            desiredVelocity = velocity;
        }

        public void UpdateStick(float avgVel, float forwardPos)
        {
            float prevPos = stickPos;
            
            avgStickVel = avgVel;
            stickPos = forwardPos;
            
            float delta = stickPos - prevPos;
            
            float hitDis = StickSweep(delta);
            
            if (hitDis < delta)
                stickPos = prevPos + hitDis;
            
            stick.localPosition = new Vector3(stick.localPosition.x, stick.localPosition.y, stickPos);
        }

        private float StickSweep(float delta)
        {
            if (delta <= 0)
                return delta;
            
            float radius = stick.localScale.x / 2f;
            Vector3 origin = stickPoint.position;
            Vector3 direction = stick.up;

            Debug.DrawRay(origin, direction * delta, Color.red, 0.1f);

            if (!Physics.SphereCast(origin, radius, direction, out RaycastHit hitInfo, delta, sweepMask))
                return delta;
            
            if (hitInfo.transform.TryGetComponent(out BallView ballView))
                ballView.AddForce(direction * avgStickVel, hitInfo.point);
            
            return delta - hitInfo.distance;
        }
    }
}