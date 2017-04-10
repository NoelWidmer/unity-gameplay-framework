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

        private static readonly object _sceneLock = new object();

        private static AsyncOperation _sceneLoader;


        public static event EventHandler PreLoadScene;
        public static event EventHandler PostLoadScene;


        public static bool IsLoadingScene
        {
            get
            {
                return _sceneLoader != null && _sceneLoader.isDone == false;
            }
        }

        public static float? SceneLoadingProgress
        {
            get
            {
                AsyncOperation sceneLoader = _sceneLoader;
                return sceneLoader == null ? default(float?) : sceneLoader.progress;
            }
        }



        public static void LoadScene(SceneName scene)
        {
            lock(_sceneLock)
            {
                if(_sceneLoader != null)
                    throw new InvalidOperationException("Only a single scene can be loaded at a time.");

                string sceneName = Enum.GetName(typeof(SceneName), scene);

                var preLoadScene = PreLoadScene;
                if(preLoadScene != null)
                    preLoadScene(null, EventArgs.Empty);

                _sceneLoader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            }
        }

        public static void Tick(float deltaTime)
        {
            AsyncOperation sceneLoader = _sceneLoader;

            if(sceneLoader != null && sceneLoader.isDone)
            {
                lock(_sceneLock)
                {
                    _sceneLoader = null;

                    var postLoadScene = PostLoadScene;
                    if(postLoadScene != null)
                        postLoadScene(null, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}