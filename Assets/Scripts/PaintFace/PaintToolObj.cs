using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PaintToolObj : MonoBehaviour
{
    [SerializeField] private BrushScriptableObject Brush;
    [SerializeField] private int ColorChannel;
    [SerializeField] private string PaintToolName;
    [SerializeField] [TextArea()] private string PaintToolInfo;
    public UI_Label_PaintToolLabel PaintToolLabel;

    public Renderer ColorRenderer;
    public Renderer OutLine;

    private float timer;

    void Start()
    {
        InitPaintTool();
    }

    void InitPaintTool()
    {
        if (ColorChannel >= 0)
        {
            ColorRenderer.material.color = PaintLevelManager.Instance.CurrentPalette[ColorChannel];
        }
        OutLine.gameObject.SetActive(false);
    }
    void ShowPaintToolInfo()
    {

        PaintToolLabel.ShowLabel(PaintToolName, PaintToolInfo, transform.position);
    }
    public void OnMouseEnter()
    {
        Debug.LogError("PointEnter");

        OutLine.gameObject.SetActive(true);
        timer = Time.time;

        ShowPaintToolInfo();
    }
    public void OnMouseExit()
    {
        Debug.LogError("PointExit");
        PaintToolLabel.HideLabel();
        OutLine.gameObject.SetActive(false);
    }
    public void OnMouseClick()
    {
        PaintToolLabel.HideLabel();
        OutLine.gameObject.SetActive(false);
        
        PaintLevelManager.Instance.SetCurrentBrush(Brush,ColorChannel);
        CameraController.Instance.ApplyCameraData(0,0.5f);
    }
}
