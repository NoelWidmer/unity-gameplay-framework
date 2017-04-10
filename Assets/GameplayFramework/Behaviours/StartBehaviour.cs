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



        protected virtual void Awake()
        {
            Game.LoadScene(StartScene);
        }
    }
}