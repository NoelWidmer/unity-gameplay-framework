using System;
using UnityEngine;

namespace GameplayFramework
{
    public class Anchor : MonoBehaviour
    {
        public event EventHandler Tick;

        protected virtual void Update()
        {
            var tick = Tick;
            if(tick != null)
                tick(this, EventArgs.Empty);
        }
    }
}