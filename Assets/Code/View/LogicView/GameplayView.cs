using System;
using Architecture.Logic;
using ImageCampus.ToolBox.Updateable;
using UnityEngine;
using View.LogicView.EntitiesView;

namespace View.LogicView
{
    public class GameplayView : MonoBehaviour, IInitable, ITickable, IDisposable
    {
        [SerializeField] private ConsoleView         consoleView;
        [SerializeField] private BallsViewController ballsViewController;

        private Gameplay gameplay;

        private void Awake()
        {
            gameplay = new Gameplay();
        }

        private void Start()
        {
            Init();
            LateInit();
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        private void OnDestroy()
        {
            gameplay.Dispose();
        }

        public void Init()
        {
            consoleView.Init();
            ballsViewController.Init();
            gameplay.Init();
        }

        public void LateInit()
        {
            consoleView.LateInit();
            ballsViewController.LateInit();
            gameplay.LateInit();
        }

        public void Tick(float deltaTime)
        {
            ballsViewController.Tick(deltaTime);
            gameplay.Tick(deltaTime);
        }

        public void Dispose()
        {
            ballsViewController.Dispose();
            gameplay.Dispose();
        }
    }
}