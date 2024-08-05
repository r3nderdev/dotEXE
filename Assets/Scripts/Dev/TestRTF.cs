using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRTF : MonoBehaviour
{

    // Jesus Christ
    void Start()
    {
        CheckFormat("ARGB32");
        CheckFormat("Depth");
        CheckFormat("ARGBHalf");
        CheckFormat("Shadowmap");
        CheckFormat("RGB565");
        CheckFormat("ARGB4444");
        CheckFormat("ARGB1555");
        CheckFormat("Default");
        CheckFormat("ARGB2101010");
        CheckFormat("DefaultHDR");
        CheckFormat("ARGB64");
        CheckFormat("ARGBFloat");
        CheckFormat("RGFloat");
        CheckFormat("RGHalf");
        CheckFormat("RFloat");
        CheckFormat("RHalf");
        CheckFormat("R8");
        CheckFormat("ARGBInt");
        CheckFormat("RGInt");
        CheckFormat("RInt");
        CheckFormat("BGRA32");
        CheckFormat("RGB111110Float");
        CheckFormat("RG32");
        CheckFormat("RGBAUShort");
        CheckFormat("RG16");
        CheckFormat("BGRA10101010_XR");
        CheckFormat("BGR101010_XR");
        CheckFormat("R16");
    }

    public bool CheckFormat(string formatString)
    {
        RenderTextureFormat formatEnum = (RenderTextureFormat)Enum.Parse(typeof(RenderTextureFormat), formatString);
        bool isSupported = SystemInfo.SupportsRandomWriteOnRenderTextureFormat(formatEnum);
        Debug.Log("Format: " + formatString + ", Supported: " + isSupported);
        return isSupported;
    }
}
