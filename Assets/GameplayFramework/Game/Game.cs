using System;
using UnityEngine;

namespace GameplayFramework
{
    public static class Game
    {
        private static GameMode _gameMode;
        private static GameState _gameState;

        public static GameMode GameMode
        {
            get
            {
                return _gameMode;
            }
        }

        public static GameState GameState
        {
            get
            {
                return _gameState;
            }
        }
    }
}