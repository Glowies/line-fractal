using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public float Speed = 0;

    // Update is called once per frame
    void Update()
    {
        var scrollDelta = Input.mouseScrollDelta.y / 16f;

        if(scrollDelta != 0)
        {
            Speed += scrollDelta;
            Speed = Mathf.Clamp(Speed, -3f, 5f);
            Time.timeScale = Mathf.Pow(2, Speed);
        }
    }
}
