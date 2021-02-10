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
    public UI_ChooseFacialPattenPanel UI_ChooseFacialPattenPanel;
    public Renderer FaceRenderer;

    //当前绘制的脸谱
    public FacialPattenScriptableObject CurrentFacialPatten => currentFacialPatten;
    private FacialPattenScriptableObject currentFacialPatten;
    public void SetCurrentFace(FacialPattenScriptableObject facialPatten)
    {
        currentFacialPatten = facialPatten;
        UI_ChooseFacialPattenPanel.SetFacialPattenInfo(facialPatten);
        SetPalette(facialPatten.MainColor);
        SetFacialPattenTexture(facialPatten.FacialPattenTexture);
        //TODO:设置角色模型的脸谱贴图
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
    
    public void SetFacialPattenTexture(Texture texture)
    {
        FaceRenderer.material.SetTexture("_FacialPattenTex",texture);
        FaceRenderer.material.SetFloat("_FacialPattenIntensity",0);
        DOTween.Kill(FaceRenderer.material);
        FaceRenderer.material.DOFloat(0.75f,"_FacialPattenIntensity",0.6f);
    }

    private void OnValidate()
    {
        SetPaletteInShader();
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