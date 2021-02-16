using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_BrushColor : MonoBehaviour
{
    [SerializeField] private BrushScriptableObject brush;
    [SerializeField] private int colorChannel;
    private Image image;
    private Button button;
    private UI_Panel_PaintTool panel;
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        image = GetComponent<Image>();
        image.color = PaintLevelManager.Instance.CurrentPalette[colorChannel];
    }
    public void OnClick()
    {
        PaintLevelManager.Instance.SetCurrentBrush(brush, colorChannel);

    }
}
