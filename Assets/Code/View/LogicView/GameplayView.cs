using Architecture.Logic;
using UnityEngine;

namespace View.LogicView
{
    public class GameplayView : MonoBehaviour
    {
        private Gameplay gameplay;

        private void Awake()
        {
            gameplay = new Gameplay();
        }

        private void Start()
        {
            gameplay.Init();
            
            gameplay.LateInit();
        }
        
        private void Update()
        {
            gameplay.Tick(Time.deltaTime);
        }
        
        private void OnDestroy()
        {
            gameplay.Dispose();
        }
    }
}