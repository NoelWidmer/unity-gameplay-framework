using System;
using UnityEngine;

namespace GameplayFramework
{
    public class GameMode
    {
        public virtual void BeginMode()
        {
            Game.TickGameMode += Tick;
            Game.GameState = new GameState();
        }



        protected virtual void Tick(TickArgs e)
        {
        }



        public virtual void EndMode()
        {
            Game.TickGameMode -= Tick;
        }
    }
}