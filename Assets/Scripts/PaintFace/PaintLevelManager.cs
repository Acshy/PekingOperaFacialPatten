using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PaintLevelManager : MonoBehaviour
{
    public static PaintLevelManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [ReadOnly] public PaintLevelGameState PaintLevelGameState;

    //绘制的脸谱
    public FacialPattenScriptableObject CurrentFacialPatten => currentFacialPatten;
    private FacialPattenScriptableObject currentFacialPatten;
    public PaintAreaHintController PaintAreaHintController;
    public void SetCurrentFace(FacialPattenScriptableObject facialPatten)
    {
        if (facialPatten != null)
        {
            currentFacialPatten = facialPatten;
            SetPalette(facialPatten.MainColor);
            FaceTextureManager.Instance.SetFacialPattenTexture(facialPatten.FacialPattenTexture,facialPatten.MaskMap1,facialPatten.MaskMap2);
        }
        else
        {
            currentFacialPatten = null;
            FaceTextureManager.Instance.SetFacialPattenTexture(null,null,null);
        }
    }

    //辅助绘制参数

    public float Correction = 0.25f;



    //当前笔刷
    public Brush CurrentBrush;
    public void SetCurrentBrush(BrushScriptableObject brushScriptable, int colorChannel)
    {
        CurrentBrush = new Brush(brushScriptable, colorChannel);
        Shader.SetGlobalInt("_CurrentChannel",colorChannel);
        if (colorChannel >= 0)
        {
            PaintAreaHintController.SetHint(true);
        }
    }

    //当前调色板
    public Color[] CurrentPalette;
    //private Color[] currentPalette;
    void SetPalette(MainColor mainColor)
    {
        if (mainColor == MainColor.Purple)
        {
            Color temp = CurrentPalette[7];
            CurrentPalette[7] = CurrentPalette[8];
            CurrentPalette[8] = temp;
        }
        SetPaletteInShader();
    }
    public void SetPaletteInShader()
    {
        Matrix4x4 channelColorsMat = new Matrix4x4(CurrentPalette[0], CurrentPalette[1], CurrentPalette[2], CurrentPalette[3]);
        Shader.SetGlobalMatrix("_MaskColor1", channelColorsMat);
        channelColorsMat = new Matrix4x4(CurrentPalette[4], CurrentPalette[5], CurrentPalette[6], CurrentPalette[7]);
        Shader.SetGlobalMatrix("_MaskColor2", channelColorsMat);
    }
    
    public void InitFaceTextureManager()
    {
        Renderer FaceRenderer = FaceTextureManager.Instance.FaceRenderer;     
        DOTween.Kill(FaceRenderer.material);
        FaceRenderer.material.DOFloat(0, "_FacialPattenIntensity",0.6f);
        FaceTextureManager.Instance.Initialized();
    }


    void Start()
    {
        SetPaletteInShader();
    }
}

public enum PaintLevelGameState
{
    Choose = 0,
    Paint = 1,
    Show = 2
}