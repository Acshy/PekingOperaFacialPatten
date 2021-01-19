using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public FacialPatten CurrentFacialPatten => currentFacialPatten;
    private FacialPatten currentFacialPatten;
    public void SetCurrentFace(FacialPatten facialPatten)
    {
        currentFacialPatten = facialPatten;
        UI_ChooseFacialPattenPanel.SetFacialPattenInfo(facialPatten);
        SetPalette(facialPatten.Palette);
        //TODO:设置角色模型的脸谱贴图
    }






    //当前调色板
    public Color[] CurrentPalette ;//=> CurrentPalette;
    //[SerializeField] private Color[] CurrentPalette;
    void SetPalette(Color[] palette)
    {
        CurrentPalette = palette;
        SetPaletteInShader();
    }
    public void SetPaletteInShader()
    {
        Matrix4x4 channelColorsMat = new Matrix4x4(CurrentPalette[0], CurrentPalette[1], CurrentPalette[2], CurrentPalette[3]);
        Shader.SetGlobalMatrix("_MaskColor1", channelColorsMat);
        CurrentPalette[7] = Color.black;//第八个通道用来抹油，不上色
        channelColorsMat = new Matrix4x4(CurrentPalette[4], CurrentPalette[5], CurrentPalette[6], CurrentPalette[7]);
        Shader.SetGlobalMatrix("_MaskColor2", channelColorsMat);
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