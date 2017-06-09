using InspectorReflector;
using UnityEngine;

namespace GameplayFramework
{
    [EnableIR]
    public class StartBehaviour : MonoSingleton
    {
        [SerializeField]
        private SceneName _startScene;
        [Inspect]
        public SceneName StartScene
        {
            get
            {
                return _startScene;
            }
            set
            {
                _startScene = value;
            }
        }

        [SerializeField]
        private GameModeName _startGameMode;
        [Inspect]
        public GameModeName StartGameMode
        {
            get
            {
                return _startGameMode;
            }
            set
            {
                _startGameMode = value;
            }
        }



        private void Awake()
        {
            DontDestroyOnLoad(this);

            // Create root GO.
            DontDestroyOnLoad(gameObject);
            gameObject.name = "GameplayFramework";

            CreateNewGame();

            // Create Anchor.
            {
                var anchorGo = new GameObject("Anchor");
                anchorGo.hideFlags = HideFlags.HideInHierarchy;
                anchorGo.transform.parent = gameObject.transform;
                Anchor anchor = anchorGo.AddComponent<Anchor>();
            }

            StartGame();
        }



        protected virtual void CreateNewGame()
        {
            Game.StartNew<Game>();
        }



        protected virtual void StartGame()
        {
            // Initialize Game.
            Game.ScenePreLoad += (sender, e) => OnScenePreLoad();
            Game.ScenePostLoad += (sender, e) => OnScenePostLoad();
            Game.Current.LoadScene(StartScene);
        }
        
        private void OnScenePreLoad()
        {
            Game.ScenePreLoad -= (sender, e) => OnScenePreLoad();
            Game.Current.SetGameMode(StartGameMode);
        }
        
        private void OnScenePostLoad()
        {
            Game.ScenePostLoad -= (sender2, e2) => OnScenePostLoad();
            Destroy(this);
        }
    }
}