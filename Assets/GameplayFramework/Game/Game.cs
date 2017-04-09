using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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



        #region Map loading

        private static readonly object _mapLock = new object();

        private static AsyncOperation _mapLoader;


        public static event EventHandler PreLoadMap;
        public static event EventHandler PostLoadMap;


        public static bool IsLoadingMap
        {
            get
            {
                return _mapLoader != null && _mapLoader.isDone == false;
            }
        }

        public static float? MapLoadingProgress
        {
            get
            {
                AsyncOperation mapLoader = _mapLoader;
                return mapLoader == null ? default(float?) : mapLoader.progress;
            }
        }



        public static void LoadMap(Map map)
        {
            lock(_mapLock)
            {
                if(_mapLoader != null)
                    throw new InvalidOperationException("Only a single map can be loaded at a time.");

                string sceneName = Enum.GetName(typeof(Map), map);

                var preLoadMap = PreLoadMap;
                if(preLoadMap != null)
                    preLoadMap(null, EventArgs.Empty);

                _mapLoader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            }
        }

        public static void Tick(float deltaTime)
        {
            AsyncOperation mapLoader = _mapLoader;

            if(mapLoader != null && mapLoader.isDone)
            {
                lock(_mapLock)
                {
                    _mapLoader = null;

                    var postLoadMap = PostLoadMap;
                    if(postLoadMap != null)
                        postLoadMap(null, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}