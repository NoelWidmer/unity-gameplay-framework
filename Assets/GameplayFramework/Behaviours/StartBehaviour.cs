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



        private void Awake()
        {
            DontDestroyOnLoad(this);

            // Create root GO.
            var gf = new GameObject("GameplayFramework");
            DontDestroyOnLoad(gf);

            // Create Anchor.
            Anchor anchor;
            {
                var anchorGo = new GameObject("Anchor");
                anchorGo.hideFlags = HideFlags.HideInHierarchy;
                anchorGo.transform.parent = gf.transform;

                anchor = anchorGo.AddComponent<Anchor>();
            }

            // Initialize Game.
            Game.Initialize(anchor);

            Game.Current.PreLoadScene += (sender, e) => StartScenePreLoadInternal();
            Game.Current.DuringLoadScene += (sender, e) => StartSceneDuringLoadInternal();
            Game.Current.PostLoadScene += (sender, e) => StartScenePostLoadInternal();

            Game.Current.LoadScene(StartScene);
        }



        private void StartScenePreLoadInternal()
        {
            Game.Current.PreLoadScene -= (sender, e) => StartScenePreLoadInternal();
            StartScenePreLoad();
        }



        private void StartSceneDuringLoadInternal()
        {
            Game.Current.DuringLoadScene -= (sender2, e2) => StartSceneDuringLoadInternal();
            StartSceneDuringLoad();
        }



        private void StartScenePostLoadInternal()
        {
            Game.Current.PostLoadScene -= (sender2, e2) => StartScenePostLoadInternal();
            StartScenePostLoad();
            Destroy(gameObject);
        }



        protected virtual void StartScenePreLoad()
        {
            Debug.Log("StartScenePreLoad");
        }



        protected virtual void StartSceneDuringLoad()
        {
            Debug.Log("StartSceneDuringLoad");
            Game.Current.SetGameMode(GameModeName.GameMode);
        }



        protected virtual void StartScenePostLoad()
        {
            Debug.Log("StartScenePostLoad");
        }
    }
}