using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayFramework
{
    public class Game
    {
        #region Singleton

        private static readonly object _instanceLock = new object();
        private static Game _instance;

        private Game()
        {
            lock(_instanceLock)
            {
                if(_instance != null)
                    throw new InvalidOperationException("The Game has already been initialized.");

                _instance = this;
            }
        }



        public static Game Current
        {
            get
            {
                return _instance;
            }
        }



        public static void Initialize(Anchor anchor)
        {
            if(anchor == null)
                throw new ArgumentNullException("anchor");

            _instance = new Game();
            anchor.Tick += (sender, e) => _instance.Tick();
        }

        #endregion

        #region GameMode and GameState

        private GameMode _gameMode;
        private GameState _gameState;

        public GameMode GameMode
        {
            get
            {
                return _gameMode;
            }
        }

        public GameState GameState
        {
            get
            {
                return _gameState;
            }
        }

        #endregion

        #region Scene loading

        private readonly object _sceneLock = new object();

        private AsyncOperation _sceneLoader;


        public event EventHandler PreLoadScene;
        public event EventHandler PostLoadScene;



        public virtual void LoadScene(SceneName scene)
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

        public virtual void Tick()
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