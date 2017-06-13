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

            Game game = GetNewGame();
            Debug.Log("About to create a new game.");
            Game.CreateNewGame(game);

            // Create Anchor.
            {
                var anchorGo = new GameObject("Anchor");
                anchorGo.hideFlags = HideFlags.HideInHierarchy;
                anchorGo.transform.parent = gameObject.transform;
                Anchor anchor = anchorGo.AddComponent<Anchor>();
            }

            Debug.Log("About to start the game.");
            StartGame();

            Destroy(this);
        }



        protected virtual Game GetNewGame()
        {
            return new Game();
        }



        protected virtual void StartGame()
        {
            Game.Current.SetGameMode(StartGameMode);
            Game.Current.LoadScene(StartScene);
        }
    }
}