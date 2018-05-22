using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class WaypointSign
{
    [SerializeField]
    private Vector2 _toConstraint = Vector2.up;
    public Vector2 ToConstraint
    {
        get
        {
            return _toConstraint;
        }
        set
        {
            _toConstraint = value;
        }
    }

    [SerializeField]
    private Vector2 _fromConstraint = Vector2.right;
    public Vector2 FromConstraint
    {
        get
        {
            return _fromConstraint;
        }
        set
        {
            _fromConstraint = value;
        }
    }

    [SerializeField]
    private Color _color = Color.yellow;
    public Color Color
    {
        get
        {
            return _color;
        }
        set
        {
            _color = new Color(value.r, value.g, value.b, .5f);
        }
    }



    public IWaypoint Waypoint
    {
        get; set;
    }



    public float GetAngle()
    {
        float angle = Vector2.Angle(ToConstraint, FromConstraint);

        Vector2 tmp = Quaternion.Euler(0, 0, 90f) * FromConstraint;
        if (Vector3.Dot(tmp.normalized, ToConstraint.normalized) < 0f)
            angle = 180f + (180f - angle);

        return angle;
    }



    public void DrawHandles(IWaypoint waypoint)
    {
        Vector3 fromConstraint3 = FromConstraint;
        Vector3 toConstraint3 = ToConstraint;

        ToConstraint = Handles.PositionHandle(toConstraint3 + waypoint.Position, Quaternion.identity) - waypoint.Position;
        FromConstraint = Handles.PositionHandle(fromConstraint3 + waypoint.Position, Quaternion.identity) - waypoint.Position;

        float angle = GetAngle();

        Handles.color = Color;
        Handles.DrawSolidArc(waypoint.Position, Vector3.forward, FromConstraint, angle, 1f);
        Handles.DrawLine(waypoint.Position, waypoint.Position + fromConstraint3);
        Handles.DrawLine(waypoint.Position, waypoint.Position + toConstraint3);
    }
}