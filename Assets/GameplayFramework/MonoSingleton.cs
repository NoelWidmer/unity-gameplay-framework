using System;
using UnityEngine;

namespace GameplayFramework
{
    public abstract class MonoSingleton : MonoBehaviour
    {
        private static readonly object _instanceLock = new object();
        private static MonoSingleton _instance;



        protected MonoSingleton Instance
        {
            get
            {
                return _instance;
            }
        }



        private void Awake()
        {
            lock(_instanceLock)
            {
                if(_instance != null)
                    throw new InvalidOperationException("The '" + GetType().Name + "' can only be instanciated once.");

                _instance = this;
            }
        }
    }
}