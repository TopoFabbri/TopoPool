using System;
using Architecture.Logic;
using ImageCampus.ToolBox.ServiceProvider;
using ImageCampus.ToolBox.Updateable;
using UnityEngine;
using View.LogicView.Controllers;
using View.LogicView.Input;

namespace View.LogicView
{
    public class GameplayView : MonoBehaviour, IInitable, ITickable, IDisposable
    {
        [SerializeField] private ConsoleView           consoleView;
        [SerializeField] private BallsViewController   ballsViewController;
        [SerializeField] private PlayersViewController playersViewController;
        [SerializeField] private LayerMask             pointerMask;

        private Gameplay gameplay;

        private ServiceProvider ServiceProvider => ServiceProvider.Instance;

        private void Awake()
        {
            gameplay = new Gameplay(Application.persistentDataPath);
            ServiceProvider.AddService<WorldCursor>(new WorldCursor(pointerMask));
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
            playersViewController.Init();
            ballsViewController.Init();
            gameplay.Init();
        }

        public void LateInit()
        {
            consoleView.LateInit();
            playersViewController.LateInit();
            ballsViewController.LateInit();
            gameplay.LateInit();
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Tick(float deltaTime)
        {
            playersViewController.Tick(deltaTime);
            ballsViewController.Tick(deltaTime);
            gameplay.Tick(deltaTime);
        }

        public void Dispose()
        {
            playersViewController.Dispose();
            ballsViewController.Dispose();
            gameplay.Dispose();
        }
    }
}