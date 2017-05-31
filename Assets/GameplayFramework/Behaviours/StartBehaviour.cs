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
        protected SceneName StartScene
        {
            get
            {
                return _startScene;
            }
        }


        [SerializeField]
        private GameModeName _startGameMode;
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

            Game world = GetNewWorld();

            // Create Anchor.
            {
                var anchorGo = new GameObject("Anchor");
                anchorGo.hideFlags = HideFlags.HideInHierarchy;
                anchorGo.transform.parent = gf.transform;
                Anchor anchor = anchorGo.AddComponent<Anchor>();
                anchor.World = world;
            }

            StartWorld(world);
        }



        protected virtual Game GetNewWorld()
        {
            return Game.BigBang<Game>();
        }



        protected virtual void StartWorld(Game world)
        {
            Debug.Log(GetType().Name + " is starting the world.");

            // Initialize World.
            Game.ScenePostLoad += (sender, e) => OnScenePostLoad();
            Game.ScenePreLoad += (sender, e) => OnScenePreLoad();
            Game.LoadScene(StartScene);
        }
        
        private void OnScenePreLoad()
        {
            Game.ScenePreLoad -= (sender, e) => OnScenePreLoad();
            Game.SetGameMode(StartGameMode);
        }
        
        private void OnScenePostLoad()
        {
            Game.ScenePostLoad -= (sender2, e2) => OnScenePostLoad();
            Destroy(gameObject);
        }
    }
}