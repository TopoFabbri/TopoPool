using ImageCampus.ToolBox.ServiceProvider;
using UnityEngine;
using UnityEngine.InputSystem;

namespace View.LogicView.Input
{
    internal sealed class WorldCursor : IService
    {
        public bool IsPersistant => true;

        private LayerMask LayerMask { get; }

        public WorldCursor(LayerMask layerMask)
        {
            LayerMask = layerMask;
        }
        
        public GameObject GetPointedObject()
        {
            Camera mainCamera = Camera.main;
            
            if (!mainCamera) return null;

            Ray ray = mainCamera.ScreenPointToRay(Pointer.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.value))
                return hit.collider.gameObject;

            return null;
        }
    }
}
