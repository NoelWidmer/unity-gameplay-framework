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
            anchor.TickMode += Tick;

            Debug.Log("Initializing GameMode: " + GetType().Name);
        }




        public virtual void BeginMode()
        {
            Debug.Log("Begin GameMode: " + GetType().Name);
            Game.Current.GameState = new GameState();
        }



        protected virtual void Tick(TickArgs e)
        {
        }



        public virtual void EndMode()
        {
            Debug.Log("End GameMode: " + GetType().Name);
            _anchor.TickMode -= Tick;
        }
    }
}