using System;
using UnityEngine;

namespace GameplayFramework
{
    public class StartBehaviour : MonoBehaviour
    {
        #region Singleton

        private static readonly object _instanceLock = new object();
        private static StartBehaviour _instance;

        public StartBehaviour()
        {
            lock(_instanceLock)
            {
                if(_instance != null)
                    throw new InvalidOperationException("The '" + typeof(StartBehaviour).Name + "' can only be instanciated once.");

                _instance = this;
            }
        }

        #endregion

        [SerializeField]
        private SceneName _startScene;

        [SerializeField]
        private GameModeName _startGameMode;



        protected SceneName StartScene
        {
            get
            {
                return _startScene;
            }
        }

        protected GameModeName StartGameMode
        {
            get
            {
                return _startGameMode;
            }
        }



        private void Awake()
        {
            DontDestroyOnLoad(this);

            // Create root GO.
            var gf = new GameObject("GameplayFramework");
            DontDestroyOnLoad(gf);

            // Create Anchor.
            {
                var anchorGo = new GameObject("Anchor");
                anchorGo.hideFlags = HideFlags.HideInHierarchy;
                anchorGo.transform.parent = gf.transform;
                anchorGo.AddComponent<Anchor>();
            }

            StartGame();
        }



        protected virtual void StartGame()
        {
            // Initialize Game.
            Game.Initialize(new Game());
            Game.ScenePostLoad += (sender, e) => OnScenePostLoad();
            Game.ScenePreLoad += (sender, e) => OnScenePreLoad();
            Game.LoadScene(StartScene);
        }
        
        private void OnScenePreLoad()
        {
            Game.ScenePreLoad -= (sender, e) => OnScenePreLoad();
            Game.SetGameMode(GameModeName.GameMode);
        }
        
        private void OnScenePostLoad()
        {
            Game.ScenePostLoad -= (sender2, e2) => OnScenePostLoad();
            Destroy(gameObject);
        }
    }
}