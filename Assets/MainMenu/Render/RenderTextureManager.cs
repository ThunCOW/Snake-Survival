using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderTextureManager : MonoBehaviour
{
    public RawImage UIImageRef;

    public static List<RenderTexture> RenderTextures;

    private Camera uiCamera;
    private RenderTexture renderTexture;

    void Start()
    {
        uiCamera = GetComponent<Camera>();

        renderTexture = new RenderTexture(1920, 1080, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm, UnityEngine.Experimental.Rendering.GraphicsFormat.D32_SFloat_S8_UInt);
        //renderTexture = UIImageRef.texture as RenderTexture;

        uiCamera.targetTexture = renderTexture;
        UIImageRef.texture = renderTexture;
        UIImageRef.color = Color.white;

        RenderTextures = new List<RenderTexture>();
    }

    void Update()
    {
        if (uiCamera == null)
            uiCamera = GetComponent<Camera>();

        uiCamera.targetTexture = (RenderTexture)UIImageRef.mainTexture;
        UIImageRef.texture = uiCamera.targetTexture;

        if (renderTexture == null)
        {
            Debug.LogError("Set Render Texture Prefab!");
            return;
        }

        //if (uiCamera.targetTexture != renderTexture)
        //    uiCamera.targetTexture = renderTexture;


        if (!RenderTextures.Contains(renderTexture))
            RenderTextures.Add(renderTexture);
    }

    public static void ReleaseRenderTextures()
    {
        foreach (RenderTexture renderTexture in RenderTextures)
            renderTexture.Release();
    }
}
