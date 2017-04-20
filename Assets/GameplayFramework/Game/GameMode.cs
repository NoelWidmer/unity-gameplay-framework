using System;
using UnityEngine;

namespace GameplayFramework
{
    public class GameMode : IDisposable
    {
        public GameMode()
        {
            World.TickGameMode += Tick;
        }

        public virtual void BeginMode()
        {
            Debug.Log("GameMode.BeginMode");
            SetGameState();
        }



        protected virtual void SetGameState()
        {
            World.GameState = new GameState();
        }



        protected virtual void Tick(TickArgs e)
        {
        }



        public virtual void Dispose()
        {
            World.TickGameMode -= Tick;
        }
    }
}