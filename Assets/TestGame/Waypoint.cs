using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour, IWaypoint
{
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }



    public List<WaypointSign> WaypointSigns = new List<WaypointSign>();

    public bool TryGetWaypointInDirection(Vector2 direction, out IWaypoint waypoint)
    {
        List<WaypointSign> result = new List<WaypointSign>();

        foreach (var waypointSign in WaypointSigns)
        {
            float angle = waypointSign.GetAngle();

            Vector2 a = waypointSign.FromConstraint.normalized;
            Vector2 b = Quaternion.Euler(0, 0, angle) * a;

            float aXb = Vector2.Dot(a, b);
            float aXc = Vector2.Dot(a, direction);
            float cXb = Vector2.Dot(direction, b);
            float cXa = Vector2.Dot(direction, a);

            if (aXb * aXc >= 0f && cXb * cXa >= 0f && angle <= 180f)
                result.Add(waypointSign);
        }

        if (result.Count == 1)
        {
            waypoint = result[0].Waypoint;
            return false;
        }
        else
        {
            waypoint = null;
            return false;
        }
    }



    public void DrawSignHandles()
    {
        foreach (var waypointSign in WaypointSigns)
        {
            waypointSign.DrawHandles(this);
        }
    }
}