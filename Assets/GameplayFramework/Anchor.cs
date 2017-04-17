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

        public Game Game
        {
            get;
            set;
        }

        protected virtual void Update()
        {
            Game.OnUnityUpdate();
        }

        protected virtual void FixedUpdate()
        {
            Game.OnUnityFixedUpdate();
        }
    }
}