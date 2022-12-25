using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class LineDrawer: MonoBehaviour
{
    public abstract int Count { get; }
    public abstract IEnumerable<Vector3> GetAllPoints();
    public abstract Vector3 GetPoint(int index);
    public abstract void SetPoint(int index, Vector3 point);
    public abstract void ClearPoints();
}

