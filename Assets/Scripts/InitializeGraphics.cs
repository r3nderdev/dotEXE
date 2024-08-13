using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGraphics : MonoBehaviour
{
    [Header("FPS")]
    [SerializeField] private bool useTargetFPS = false;
    [SerializeField] private int targetFrameRate = 60;

    [Header("Resolution")]
    [SerializeField] private bool useResolution = false;
    [SerializeField] private int width = 1280, height = 720;


    private void Start()
    {
        if (useTargetFPS) Application.targetFrameRate = targetFrameRate;
        else if (!useTargetFPS) Application.targetFrameRate = -1; // Platform default

        if (useResolution) Screen.SetResolution(width, height, false);
    }
}
