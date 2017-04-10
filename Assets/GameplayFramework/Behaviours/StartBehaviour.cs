using UnityEngine;

namespace GameplayFramework
{
    public class StartBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SceneName _startScene;

        public SceneName StartScene
        {
            get
            {
                return _startScene;
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
            Game.Current.PostLoadScene += (sender, e) => StartScenePostLoadInternal();

            Game.Current.LoadScene(StartScene);
        }



        private void StartScenePreLoadInternal()
        {
            Game.Current.PreLoadScene -= (sender, e) => StartScenePreLoadInternal();
            StartScenePreLoad();
        }



        private void StartScenePostLoadInternal()
        {
            Game.Current.PostLoadScene -= (sender2, e2) => StartScenePostLoadInternal();
            StartScenePostLoad();
            Destroy(gameObject);
        }



        protected virtual void StartScenePreLoad()
        {
            //TODO initialize first mode
            Debug.Log("StartScenePreLoad");
        }



        protected virtual void StartScenePostLoad()
        {
            Debug.Log("StartScenePostLoad");
        }
    }
}