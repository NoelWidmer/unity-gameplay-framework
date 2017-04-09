using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class Game
    {
        private Game _instance;

        private Game()
        {
        }

        public static void RequestJoin(PlayerController playerController, Action callback)
        {
            if(playerController == null)
                throw new ArgumentNullException();

            if(callback == null)
                throw new ArgumentNullException();

            //TODO: Handle join
        }
    }
}