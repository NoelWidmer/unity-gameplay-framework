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
        protected GameModeName StartGameMode
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

            Game game = CreateNewGame();

            // Create Anchor.
            {
                var anchorGo = new GameObject("Anchor");
                anchorGo.hideFlags = HideFlags.HideInHierarchy;
                anchorGo.transform.parent = gameObject.transform;
                Anchor anchor = anchorGo.AddComponent<Anchor>();
                anchor.Game = game;
            }

            StartGame(game);
        }



        protected virtual Game CreateNewGame()
        {
            return Game.CreateNew<Game>();
        }



        protected virtual void StartGame(Game game)
        {
            // Initialize Game.
            Game.ScenePreLoad += (sender, e) => OnScenePreLoad();
            Game.ScenePostLoad += (sender, e) => OnScenePostLoad();
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
            Destroy(this);
        }
    }
}