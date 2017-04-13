using System;
using UnityEngine;

namespace GameplayFramework
{
    public class Anchor : MonoBehaviour
    {
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



        public event EventHandler TickInput;
        public event EventHandler TickControl;
        public event EventHandler TickCamera;
        public event EventHandler TickHUD;
        public event EventHandler TickActor;
        public event EventHandler TickMode;
        public event EventHandler TickLast;

        public event EventHandler TickLate;
        public event EventHandler TickFixed;



        protected virtual void Update()
        {
            {
                var tickInput = TickInput;
                if(tickInput != null)
                    tickInput(this, EventArgs.Empty);
            }

            {
                var tickControl = TickControl;
                if(tickControl != null)
                    tickControl(this, EventArgs.Empty);
            }

            {
                var tickCamera = TickCamera;
                if(tickCamera != null)
                    tickCamera(this, EventArgs.Empty);
            }

            {
                var tickHUD = TickHUD;
                if(tickHUD != null)
                    tickHUD(this, EventArgs.Empty);
            }

            {
                var tickActor = TickActor;
                if(tickActor != null)
                    tickActor(this, EventArgs.Empty);
            }

            {
                var tickMode = TickMode;
                if(tickMode != null)
                    tickMode(this, EventArgs.Empty);
            }

            {
                var tickLast = TickLast;
                if(tickLast != null)
                    tickLast(this, EventArgs.Empty);
            }
        }

        protected virtual void LateUpdate()
        {
            {
                var tickLate = TickLate;
                if(tickLate != null)
                    tickLate(this, EventArgs.Empty);
            }
        }

        protected virtual void FixedUpdate()
        {
            {
                var tickFixed = TickFixed;
                if(tickFixed != null)
                    tickFixed(this, EventArgs.Empty);
            }
        }
    }
}