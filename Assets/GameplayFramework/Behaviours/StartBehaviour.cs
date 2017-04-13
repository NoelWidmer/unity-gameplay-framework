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

        #region Start Scene and Start Mode

        [SerializeField]
        private SceneName _startScene;

        [SerializeField]
        private GameModeName _startGameMode;



        public SceneName StartScene
        {
            get
            {
                return _startScene;
            }
        }

        public GameModeName StartGameMode
        {
            get
            {
                return _startGameMode;
            }
        }

        #endregion

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

            BeginGame();
        }



        protected virtual void BeginGame()
        {
            Debug.Log("StartBehaviour.BeginGame");

            // Initialize Game.
            Game.Initialize();

            Game.ScenePreLoad += (sender, e) => OnScenePreLoad();
            Game.ScenePostLoad += (sender, e) => OnScenePostLoad();

            Game.LoadScene(StartScene);
        }



        private void OnScenePreLoad()
        {
            Debug.Log("StartBehaviour.OnScenePreLoad");

            Game.ScenePreLoad -= (sender, e) => OnScenePreLoad();
            Game.SetGameMode(GameModeName.GameMode);
        }



        private void OnScenePostLoad()
        {
            Debug.Log("StartBehaviour.OnScenePostLoad");

            Game.ScenePostLoad -= (sender2, e2) => OnScenePostLoad();
            Destroy(gameObject);
        }
    }
}