using System;
using System.Collections.Generic;
using System.Numerics;
using Architecture.Logic.Entities;
using ImageCampus.ToolBox.Updateable;

namespace Architecture.Logic
{
    public sealed class Scene : IInitable, ITickable, IDisposable
    {
        private readonly int ballCount;
        
        private readonly List<Ball> balls = new();

        public Scene(int ballCount)
        {
            this.ballCount = ballCount;
        }
        
        public void Init()
        {
            for (int i = 0; i < ballCount; i++)
                balls.Add(new Ball(i >= ballCount / 2, Vector3.Zero + Vector3.UnitX * i * 0.1f + Vector3.UnitY, (uint)i));
            
            foreach (Ball ball in balls)
                ball.Init();
        }

        public void LateInit()
        {
            foreach (Ball ball in balls)
                ball.LateInit();
        }

        public void Tick(float deltaTime)
        {
            foreach (Ball ball in balls)
                ball.Tick(deltaTime);
        }

        public void Dispose()
        {
            foreach (Ball ball in balls)
                ball.Dispose();
        }
    }
}