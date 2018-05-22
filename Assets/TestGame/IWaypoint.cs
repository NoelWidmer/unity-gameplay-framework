using UnityEngine;

public interface IWaypoint
{
    Vector3 Position
    {
        get; set;
    }

    bool TryGetWaypointInDirection(Vector2 direction, out IWaypoint waypoint);

    void DrawSignHandles();
}