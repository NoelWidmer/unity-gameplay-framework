using System;
using UnityEngine;

namespace GameplayFramework
{
    public class GameMode : IDisposable
    {
        public GameMode()
        {
            Game.TickGameMode += Tick;
        }

        public virtual void BeginMode()
        {
            Debug.Log("GameMode.BeginMode");
            SetGameState();
        }



        protected virtual void SetGameState()
        {
            Game.GameState = new GameState();
        }



        protected virtual void Tick(TickArgs e)
        {
        }



        public virtual void Dispose()
        {
            Game.TickGameMode -= Tick;
        }
    }
}