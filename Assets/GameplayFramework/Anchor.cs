using System;
using System.Diagnostics;
using UnityEngine;

namespace GameplayFramework
{
    public class Anchor : MonoBehaviour
    {
        private readonly Stopwatch _normalWatch = new Stopwatch();
        private readonly Stopwatch _lateWatch = new Stopwatch();
        private readonly Stopwatch _fixedWatch = new Stopwatch();



        #region Singleton

        private static readonly object _instanceLock = new object();
        private static Anchor _instance;

        public Anchor()
        {
            lock(_instanceLock)
            {
                if(_instance != null)
                    throw new InvalidOperationException("The '" + typeof(Anchor).Name + "' can only be instanciated once.");

                _instance = this;
            }
        }

        #endregion


        private float _gameTime;

        public event TickHandler TickInput;
        public event TickHandler TickControl;
        public event TickHandler TickCamera;
        public event TickHandler TickHUD;
        public event TickHandler TickActor;
        public event TickHandler TickMode;
        public event TickHandler TickLast;

        public event TickHandler TickLate;
        public event TickHandler TickFixed;



        protected virtual void Awake()
        {
            _gameTime = Time.time;

            _normalWatch.Start();
            _lateWatch.Start();
            _fixedWatch.Start();
        }



        protected virtual void Update()
        {
            float deltaTime = _normalWatch.Elapsed.Milliseconds / 1000f;
            _normalWatch.Reset();
            
            TickArgs tickArgs = new TickArgs(deltaTime);
            _gameTime += deltaTime;

            {
                var tickInput = TickInput;
                if(tickInput != null)
                    tickInput(tickArgs);
            }

            {
                var tickControl = TickControl;
                if(tickControl != null)
                    tickControl(tickArgs);
            }

            {
                var tickCamera = TickCamera;
                if(tickCamera != null)
                    tickCamera(tickArgs);
            }

            {
                var tickHUD = TickHUD;
                if(tickHUD != null)
                    tickHUD(tickArgs);
            }

            {
                var tickActor = TickActor;
                if(tickActor != null)
                    tickActor(tickArgs);
            }

            {
                var tickMode = TickMode;
                if(tickMode != null)
                    tickMode(tickArgs);
            }

            {
                var tickLast = TickLast;
                if(tickLast != null)
                    tickLast(tickArgs);
            }
        }

        protected virtual void LateUpdate()
        {
            float deltaTime = _lateWatch.Elapsed.Milliseconds / 1000f;
            _normalWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);

            {
                var tickLate = TickLate;
                if(tickLate != null)
                    tickLate(tickArgs);
            }
        }

        protected virtual void FixedUpdate()
        {
            float deltaTime = _fixedWatch.Elapsed.Milliseconds / 1000f;
            _normalWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);

            {
                var tickFixed = TickFixed;
                if(tickFixed != null)
                    tickFixed(tickArgs);
            }
        }
    }
}