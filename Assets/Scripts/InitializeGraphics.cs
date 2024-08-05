using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGraphics : MonoBehaviour
{
    [SerializeField] private bool useTargetFPS = false;
    [SerializeField] private int targetFrameRate;
    [SerializeField] private bool setResolution = false;


    private void Awake()
    {
        if (useTargetFPS) Application.targetFrameRate = targetFrameRate;
        else if (!useTargetFPS) Application.targetFrameRate = -1; // Platform default

        if (setResolution) Screen.SetResolution(1280, 720, true);
    }
}
