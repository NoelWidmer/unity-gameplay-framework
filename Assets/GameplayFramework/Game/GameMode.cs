using System;
using UnityEngine;

namespace GameplayFramework
{
    public class GameMode
    {
        public virtual void Initialize(Anchor anchor)
        {
            if(anchor == null)
                throw new ArgumentNullException("anchor");

            Debug.Log("Initializing GameMode: " + GetType().Name);
        }



        public virtual void EndMode()
        {
            Debug.Log("End GameMode: " + GetType().Name);
        }




        public virtual void BeginMode()
        {
            Debug.Log("Begin GameMode: " + GetType().Name);
        }
    }
}