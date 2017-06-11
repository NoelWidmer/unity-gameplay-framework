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
            Debug.Log("Create new game.");
            Game.Current = new Game();
        }



        protected virtual void StartGame()
        {
            // Initialize Game.
            Debug.Log("Start game.");

            Game.Current.SetGameMode(StartGameMode);
            Game.Current.LoadScene(StartScene);

            Destroy(this);
        }
    }
}