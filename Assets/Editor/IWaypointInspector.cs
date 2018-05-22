using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class PositionHandleExampleEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        IWaypoint example = (IWaypoint)target;
        example.DrawSignHandles();
    }
}