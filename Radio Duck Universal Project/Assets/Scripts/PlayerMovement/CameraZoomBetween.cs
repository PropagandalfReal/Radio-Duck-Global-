using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomBetween : MonoBehaviour
{
    public List<Transform> Targets;

    public Vector3 Offset;
    void LateUpdate()
    {
        if (Targets.Count == 0)
            return;
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + Offset;

        transform.position = centerPoint;
    }
    Vector3 GetCenterPoint()
    {
        if (Targets.Count == 1)
        {
            return Targets[0].position;
        }

        var NewBounds = new Bounds(Targets[0].position, Vector3.zero);
        for (int i = 0; i < Targets.Count; i++)
        {
            NewBounds.Encapsulate(Targets[i].position);
        }

        return NewBounds.center;

    }
}
