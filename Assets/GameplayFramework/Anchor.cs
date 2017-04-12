using System;
using UnityEngine;

namespace GameplayFramework
{
    public class Anchor : MonoBehaviour
    {
        #region Singleton

        private static readonly object _instanceLock = new object();
        private static Anchor _instance;



        private void Awake()
        {
            lock(_instanceLock)
            {
                if(_instance != null)
                {
                    Destroy(this);
                    throw new InvalidOperationException("The Anchor has already been instanciated.");
                }
                
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
    }
}