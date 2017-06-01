namespace GameplayFramework
{
    public class Anchor : MonoSingleton
    {
        public Game Game
        {
            get;
            set;
        }

        protected virtual void Update()
        {
            Game.OnUnityUpdate();
        }

        protected virtual void FixedUpdate()
        {
            Game.OnUnityFixedUpdate();
        }
    }
}