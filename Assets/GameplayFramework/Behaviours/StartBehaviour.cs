using System;
using UnityEngine;

namespace GameplayFramework
{
    public class StartBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Map _startMap;

        public Map StartMap
        {
            get
            {
                return _startMap;
            }
        }

        private void Awake()
        {
            Game.LoadMap(StartMap);
        }
    }
}