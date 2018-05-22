using UnityEditor;

namespace InspectorReflector.Implementation
{
    /// <summary>
    ///     This class provides the editor integration that allows the IR to draw its own visuals.
    /// </summary>
    [CustomEditor(typeof(object), true)]
    public class IRHook : Editor
    {
        private static object _lastTarget;



        public override void OnInspectorGUI()
        {
            object obj = target;

            IRDrawer drawer = new IRDrawer();

            if (drawer.SupportsIRInspection(obj))
            {
                if (obj == null)
                {
                    _lastTarget = null;
                    return;
                }

                if (_lastTarget == null)
                {
                    _lastTarget = obj;
                    // Set transient data to null.
                }
                else if (_lastTarget.Equals(obj) == false)
                {
                    // Create new transient data.
                }

                drawer.ReflectInspector(obj);
            }
            else
            {
                _lastTarget = null;
                DrawDefaultInspector();
            }
        }
    }
}