using System.Collections;
using UnityEngine;

namespace View.LogicView.EntitiesView
{
    internal sealed class BallView : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        
        public  uint ID { get; private set; }
        private bool solid;
        
        public BallView Spawn(uint id, bool solid, Vector3 position, Quaternion rotation, Transform parent)
        {
            BallView instance = Instantiate(this, position, rotation, parent);
            
            instance.ID = id;
            instance.solid = solid;
            
            return instance;
        }

        public void AddForce(Vector3 force, Vector3 point)
        {
            StartCoroutine(AddForceCoroutine(force, point));
        }

        private IEnumerator AddForceCoroutine(Vector3 force, Vector3 point, ForceMode mode = ForceMode.Impulse)
        {
            yield return new WaitForFixedUpdate();
            rb.AddForceAtPosition(force, point, mode);
        }
    }
}