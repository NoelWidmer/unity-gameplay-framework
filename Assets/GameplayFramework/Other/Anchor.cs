using UnityEngine;

namespace GameplayFramework
{
    public class Anchor : MonoSingleton
    {
        protected virtual void Update()
        {
            Game.Current.OnUnityUpdate();
        }

        protected virtual void FixedUpdate()
        {
            Game.Current.OnUnityFixedUpdate();
        }
    }
}