using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class URPLineDrawer : LineDrawer
{
    private LineRenderer _lineRenderer;
    const int MAX_POINT_COUNT = 1024;

    private void Awake()
    {
        TryGetComponent(out _lineRenderer);
    }

    public override int Count => _lineRenderer.positionCount;

    public override void ClearPoints()
    {
        var points = new Vector3[0];
        _lineRenderer.SetPositions(points);
        _lineRenderer.positionCount = 0;
    }

    public override IEnumerable<Vector3> GetAllPoints()
    {
        Vector3[] result = new Vector3[MAX_POINT_COUNT];
        _lineRenderer.GetPositions(result);
        return result;
    }

    public override Vector3 GetPoint(int index)
    {
        return _lineRenderer.GetPosition(index);
    }

    public override void SetPoint(int index, Vector3 point)
    {
        while (Count <= index)
        {
            _lineRenderer.positionCount = index + 1;
        }

        _lineRenderer.SetPosition(index, point);
    }
}
