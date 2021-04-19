using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTextureManager_CreateFace : MonoBehaviour
{
    public static FaceTextureManager_CreateFace Instance { get; private set; }
    public Renderer FaceRenderer;
    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        InitFaceTexture();
    }

    public void InitFaceTexture()
    {
        FaceRenderer.material.SetTexture("_FacePartTexture",null);
        FaceRenderer.material.SetTexture("_FacePartTexture2",null);
        FaceRenderer.material.SetTexture("_ForeHeadPartTexture",null);
        FaceRenderer.material.SetTexture("_BrowPartTexture",null);
        FaceRenderer.material.SetTexture("_EyePartTexture",null);
        FaceRenderer.material.SetTexture("_NosePartTexture",null);
        FaceRenderer.material.SetTexture("_MousePartTexture",null);
        FaceRenderer.material.SetTexture("_DecoPartTexture",null);
    }

    public void SetTexture(string textureName,Sprite sprite)
    {
        if(sprite!=null)
        {
            FaceRenderer.material.SetTexture(textureName, sprite.texture);
        }
        else
        {
            FaceRenderer.material.SetTexture(textureName, null);
        }
    } 
    public void SetColor(string colorName, Color color)
    {
        FaceRenderer.material.SetColor(colorName,color);
    }
}
