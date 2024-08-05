using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newASCII : MonoBehaviour
{
    public Shader asciiShader;
    public ComputeShader asciiCompute;
    public Texture asciiTex, edgeTex;

    [Range(1, 10)]
    public int gaussianKernelSize = 2;

    [Range(0.1f, 5.0f)]
    public float stdev = 2.0f;

    [Range(0.1f, 5.0f)]
    public float stdevScale = 1.6f;

    [Range(0.01f, 5.0f)]
    public float tau = 1.0f;

    [Range(0.001f, 0.1f)]
    public float threshold = 0.005f;

    public bool invert = false;

    public bool viewDog = false;
    public bool viewSobel = false;
    public bool viewGrid = false;
    public bool debugEdges = false;
    public bool viewUncompressedEdges = false;
    public bool viewQuantizedSobel = false;
    public bool noEdges = false;
    public bool noFill = false;

    [Range(0, 64)]
    public int edgeThreshold = 8;

    [Range(0.0f, 10.0f)]
    public float exposure = 1.0f;

    [Range(0.0f, 10.0f)]
    public float attenuation = 1.0f;

    private Material asciiMat;
    private RenderTexture asciiRenderTex;

    void OnEnable()
    {
        asciiMat = new Material(asciiShader);
        asciiMat.hideFlags = HideFlags.HideAndDontSave;

        if (asciiRenderTex == null)
        {
            asciiRenderTex = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            // Removed enableRandomWrite
            asciiRenderTex.Create();
        }
    }

    void Update()
    {
        if (asciiRenderTex == null || Screen.width != asciiRenderTex.width || Screen.height != asciiRenderTex.height)
        {
            asciiRenderTex = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            // Removed enableRandomWrite
            asciiRenderTex.Create();
        }
    }

    void OnDisable()
    {
        asciiMat = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        asciiMat.SetTexture("_AsciiTex", asciiTex);

        var ping = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

        var luminance = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.RHalf);
        var sobel = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
        var dog = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

        Graphics.Blit(source, luminance, asciiMat, 1); // Luminance

        asciiMat.SetFloat("_K", stdevScale);
        asciiMat.SetFloat("_Sigma", stdev);
        asciiMat.SetFloat("_Tau", tau);
        asciiMat.SetInt("_GaussianKernelSize", gaussianKernelSize);
        asciiMat.SetFloat("_Threshold", threshold);
        asciiMat.SetInt("_Invert", invert ? 1 : 0);
        Graphics.Blit(luminance, ping, asciiMat, 3); // Horizontal Blur
        Graphics.Blit(ping, dog, asciiMat, 4); // Vertical Blur and Difference

        asciiMat.SetTexture("_LuminanceTex", luminance);
        Graphics.Blit(dog, ping, asciiMat, 5); // Sobel Horizontal Pass

        Graphics.Blit(ping, sobel, asciiMat, 6); // Sobel Vertical Pass

        Graphics.Blit(source, ping, asciiMat, 2); // Pack luminance

        var downscale1 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, source.format);
        var downscale2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
        var downscale3 = RenderTexture.GetTemporary(source.width / 8, source.height / 8, 0, source.format);

        Graphics.Blit(ping, downscale1, asciiMat, 0);
        Graphics.Blit(downscale1, downscale2, asciiMat, 0);
        Graphics.Blit(downscale2, downscale3, asciiMat, 0);

        asciiCompute.SetTexture(0, "_SobelTex", sobel);
        asciiCompute.SetTexture(0, "_Result", asciiRenderTex);
        asciiCompute.SetTexture(0, "_EdgeAsciiTex", edgeTex);
        asciiCompute.SetTexture(0, "_AsciiTex", asciiTex);
        asciiCompute.SetTexture(0, "_LuminanceTex", downscale3);
        asciiCompute.SetInt("_ViewUncompressed", viewUncompressedEdges ? 1 : 0);
        asciiCompute.SetInt("_DebugEdges", debugEdges ? 1 : 0);
        asciiCompute.SetInt("_Grid", viewGrid ? 1 : 0);
        asciiCompute.SetInt("_NoEdges", noEdges ? 1 : 0);
        asciiCompute.SetInt("_NoFill", noFill ? 1 : 0);
        asciiCompute.SetInt("_EdgeThreshold", edgeThreshold);
        asciiCompute.SetFloat("_Exposure", exposure);
        asciiCompute.SetFloat("_Attenuation", attenuation);
        asciiCompute.Dispatch(0, Mathf.CeilToInt(source.width / 8), Mathf.CeilToInt(source.width / 8), 1);

        Graphics.Blit(asciiRenderTex, destination);

        if (viewDog)
            Graphics.Blit(dog, destination, asciiMat, 0);

        if (viewSobel)
            Graphics.Blit(sobel, destination, asciiMat, 0);

        if (viewQuantizedSobel || viewUncompressedEdges || debugEdges || viewGrid)
            Graphics.Blit(asciiRenderTex, destination, asciiMat, 0);

        RenderTexture.ReleaseTemporary(ping);
        RenderTexture.ReleaseTemporary(luminance);
        RenderTexture.ReleaseTemporary(sobel);
        RenderTexture.ReleaseTemporary(downscale1);
        RenderTexture.ReleaseTemporary(downscale2);
        RenderTexture.ReleaseTemporary(downscale3);
        RenderTexture.ReleaseTemporary(dog);
    }
}