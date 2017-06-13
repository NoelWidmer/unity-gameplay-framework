using InspectorReflector;
using UnityEngine;

namespace GameplayFramework
{
    [EnableIR]
    public class GameInspector : MonoBehaviour
    {
        [Inspect]
        public Game Game
        {
            get
            {
                return Game.Current;
            }
        }
    }
}