using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FractalSetting
{
    public int DivisionCount;
    public bool ReverseAngle;
}

public class Randomizer : MonoBehaviour
{
    public int StartIndex = -1;
    public FractalDrawer FractalDrawer;
    public FractalSetting[] Settings;

    // Start is called before the first frame update
    void Start()
    {
        if(StartIndex > -1)
        {
            StartFractal(StartIndex);
        }
        else
        {
            StartRandomFractal();
        }
    }

    private void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || 
            Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
        {
            StartRandomFractal();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartFractal(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartFractal(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartFractal(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartFractal(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartFractal(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartFractal(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StartFractal(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartFractal(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartFractal(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartFractal(9);
        }
    }

    [ContextMenu("Start Random Fractal")]
    public void StartRandomFractal()
    {
        int randIndex = Random.Range(0, Settings.Length);
        StartFractal(randIndex);
    }

    public void StartFractal(int index)
    {
        if(index >= Settings.Length)
        {
            return;
        }

        var selection = Settings[index];
        FractalDrawer.DivisionCount = selection.DivisionCount;
        FractalDrawer.ReverseAngle = selection.ReverseAngle;

        FractalDrawer.StartDrawing();
    }
}
