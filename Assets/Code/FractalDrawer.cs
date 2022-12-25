using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalDrawer : MonoBehaviour
{
    public int DivisionCount = 4;
    public bool ReverseAngle = false;
    public float StartAngle = 0;
    public float StartLength = 1;
    public float DrawSpeed = 5f;
    public float WaitTime = 0.08f;
    public LineDrawer LineDrawer;

    private float _angleDelta;
    private float _lengthOffset;
    private int _lineCount;

    public void StartDrawing()
    {
        StopAllCoroutines();

        // Determine Length Offset
        _lengthOffset = StartLength / DivisionCount / DivisionCount;

        // Determine Angle Delta
        _angleDelta = 2f * Mathf.PI / DivisionCount;
        if(ReverseAngle)
        {
            _angleDelta *= DivisionCount - 2;
        }

        // Create first line
        ClearPoints();
        var secondPoint = FindAngledOffset(StartAngle, StartLength);
        SetPoint(0, Vector3.zero);
        SetPoint(1, secondPoint);

        // Create the rest of the inital lines
        _lineCount = 1;
        for (;_lineCount < DivisionCount - 1; _lineCount++)
        {
            var refPoint1 = GetPoint(_lineCount);
            var refPoint0 = GetPoint(_lineCount - 1);

            var diff = refPoint1 - refPoint0;

            var prevAngle = Mathf.Atan2(diff.y, diff.x);
            var newAngle = prevAngle + _angleDelta;

            var length = StartLength + _lengthOffset * _lineCount;
            var offset = FindAngledOffset(newAngle, length);
            var newPoint = refPoint1 + offset;

            SetPoint(_lineCount + 1, newPoint);
        }

        SetCameraToAverage();

        // Start Draw Animation
        StartCoroutine(DrawOneLine());
    }

    IEnumerator DrawOneLine()
    {
        // Find target point
        Vector2 p1 = GetPoint(_lineCount);
        Vector2 p2 = GetPoint(_lineCount - DivisionCount + 2);
        Vector3 lineDir = GetPoint(_lineCount - DivisionCount + 1) - GetPoint(_lineCount);
        Vector2 d1 = lineDir.normalized;
        var prevAngle = Mathf.Atan2(d1.y, d1.x);
        var newAngle = prevAngle + _angleDelta;
        Vector2 d2 = FindAngledOffset(newAngle, 1) * -1;

        Vector2 targetPoint = FindIntersectionPoint(p1, d1, p2, d2)
            + d1 * _lengthOffset;

        float length = (targetPoint - p1).magnitude;

        // Draw current line towards target point
        float t = 0;
        while (t < 1)
        {
            t += DrawSpeed * Time.deltaTime;

            var tempPoint = Vector2.Lerp(p1, targetPoint, t);
            SetPoint(_lineCount + 1, tempPoint);

            yield return new WaitForEndOfFrame();
        }

        // Wait before next line
        yield return new WaitForSeconds(WaitTime);

        _lineCount++;

        StartCoroutine(DrawOneLine());
    }

    Vector3 FindAngledOffset(float angle, float size)
    {
        var result = new Vector3(
            Mathf.Cos(angle),
            Mathf.Sin(angle),
            0
        );

        return result * size;
    }

    private void SetCameraToAverage()
    {
        Vector3 sum = Vector3.zero;
        foreach(var point in LineDrawer.GetAllPoints())
        {
            sum += point;
        }

        var average = sum / LineDrawer.Count;
        average.z = -10;
        Camera.main.transform.position = average;
    }

    private void ClearPoints() => LineDrawer.ClearPoints();

    private Vector3 GetPoint(int index) => LineDrawer.GetPoint(index);

    private void SetPoint(int index, Vector3 position) => LineDrawer.SetPoint(index, position);

    private Vector2 FindIntersectionPoint(
        Vector2 point1,
        Vector2 dir1,
        Vector2 point2,
        Vector2 dir2
    )
    {
        var PQx = point2[0] - point1[0];
        var PQy = point2[1] - point1[1];
        var rx = dir1[0];
        var ry = dir1[1];
        var rxt = -ry;
        var ryt = rx;
        var qx = PQx * rx + PQy * ry;
        var qy = PQx * rxt + PQy * ryt;
        var sx = dir2[0] * rx + dir2[1] * ry;
        var sy = dir2[0] * rxt + dir2[1] * ryt;

        // if lines are identical or do not cross...
        if (sy == 0) return Vector2.zero;
        var a = qx - qy * sx / sy;
        return new Vector2(point1[0] + a * rx, point1[1] + a * ry);

    }
}
