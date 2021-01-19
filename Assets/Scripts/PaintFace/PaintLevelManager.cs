using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintLevelManager : MonoBehaviour
{
    public static PaintLevelManager Instance { get; private set; }
    public PaintLevelGameState PaintLevelGameState;
    public UI_ChooseFacialPattenPanel UI_ChooseFacialPattenPanel;
    private void Awake()
    {
        Instance = this;
    }
    

    public FacialPatten CurrentFacialPatten=> currentFacialPatten;
    private FacialPatten currentFacialPatten;
    public void SetCurrentFace(FacialPatten facialPatten)
    {
        currentFacialPatten = facialPatten;
        UI_ChooseFacialPattenPanel.SetFacialPattenInfo(facialPatten);
        //TODO:设置脸谱贴图
    }


    
    void Start()
    {

    }

}

public enum PaintLevelGameState
{
    Choose=0,
    Paint=1,
    Show=2
}