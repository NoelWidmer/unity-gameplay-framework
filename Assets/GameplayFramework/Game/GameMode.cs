using System;
using UnityEngine;

namespace GameplayFramework
{
    public class GameMode
    {
        public void Initialize()
        {
            Game.TickMode += Tick;
            Debug.Log("Initializing GameMode: " + GetType().Name);
        }




        public virtual void BeginMode()
        {
            Debug.Log("Begin GameMode: " + GetType().Name);
            Game.GameState = new GameState();
        }



        protected virtual void Tick(TickArgs e)
        {
        }



        public virtual void EndMode()
        {
            Debug.Log("End GameMode: " + GetType().Name);
            Game.TickMode -= Tick;
        }
    }
}