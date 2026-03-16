using UnityEngine;

namespace View.LogicView.EntitiesView
{
    internal class PlayerView : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        
        public uint ID { get; private set; }
        
        public PlayerView Spawn(uint id, bool possess, Vector3 position, Quaternion rotation, Transform parent)
        {
            PlayerView instance = Instantiate(this, position, rotation, parent);
            
            instance.ID = id;
            
            instance.cam.tag = possess ? "MainCamera" : "Untagged";
            instance.cam.enabled = possess;
            
            return instance;
        }
    }
}