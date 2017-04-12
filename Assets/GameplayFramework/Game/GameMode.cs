using System;
using UnityEngine;

namespace GameplayFramework
{
    public class GameMode
    {
        private Anchor _anchor;

        public void Initialize(Anchor anchor)
        {
            if(anchor == null)
                throw new ArgumentNullException("anchor");

            _anchor = anchor;
            anchor.TickMode += (sender, e) => Tick();

            Debug.Log("Initializing GameMode: " + GetType().Name);
        }




        public virtual void BeginMode()
        {
            Debug.Log("Begin GameMode: " + GetType().Name);
        }



        protected virtual void Tick()
        {
            Debug.Log("TODO set game state.");
        }



        public virtual void EndMode()
        {
            Debug.Log("End GameMode: " + GetType().Name);
        }
    }
}