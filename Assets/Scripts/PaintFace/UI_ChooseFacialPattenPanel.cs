using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
public class UI_ChooseFacialPattenPanel : MonoBehaviour
{
    [FoldoutGroup("左面板")] public RectTransform LeftPanel;
    [FoldoutGroup("左面板")] public GridLayoutGroup Left_FacialGroup;
    [FoldoutGroup("左面板")] public UI_FacialPattenBtn Left_FacialPatten;
    [FoldoutGroup("左面板")] public Dropdown Left_HangDang;
    [FoldoutGroup("左面板")] public Dropdown Left_MainColor;
    [FoldoutGroup("左面板")] public Dropdown Left_Struct;



    [FoldoutGroup("右面板")] public RectTransform RightPanel;
    [FoldoutGroup("右面板")] public Image Right_Image;

    [FoldoutGroup("右面板")] public Text Right_Name;

    [FoldoutGroup("右面板")] public Text Right_From;

    [FoldoutGroup("右面板")] public Text Right_TextArea;

    [FoldoutGroup("右面板")] public CanvasGroup Right_InfoStory;

    [FoldoutGroup("右面板")] public CanvasGroup Right_InfoFacialPatten;

    [FoldoutGroup("右面板")] public CanvasGroup Right_InfoPaintSkill;

    [FoldoutGroup("右面板")] public Button Right_ChooseBtn;



    public List<FacialPatten> FacialPattenList;
    // Start is called before the first frame update
    void Start()
    {
        ShowFacialPattenGroup(FacialPattenList);
        ShowPanel(true);
    }
    

    //显示/隐藏面板
    private bool isOnShow = true;
    void ShowPanel(bool show)
    {
        if (isOnShow && (!show))
        {
            RightPanel.DOAnchorPosX(360, 1);
            LeftPanel.DOAnchorPosX(-360, 1);
        }
        else if ((!isOnShow) && show)
        {
            RightPanel.DOAnchorPosX(0, 1);
            LeftPanel.DOAnchorPosX(0, 1);
        }
        isOnShow = show;
    }


    //设置脸谱信息
    public void SetFacialPattenInfo(FacialPatten facialPatten)
    {
        if (facialPatten == null)
        {
            return;
        }
        Right_Image.sprite = facialPatten.Image;
        Right_Name.text = facialPatten.FaceName;
        Right_From.text = facialPatten.From;
        Right_Name.text = facialPatten.FaceName;
        Right_TextArea.text = facialPatten.InfoStory;
    }




    //选定按钮
    public void OnClickChooseBtn()
    {
        //隐藏面板
        ShowPanel(false);
    }

    //切换显示信息
    public void OnClickInfoSwitchButton(int index)
    {
        switch (index)
        {
            case 0:
                Right_TextArea.text = PaintLevelManager.Instance.CurrentFacialPatten.InfoStory;
                Right_InfoStory.alpha=1f;
                Right_InfoFacialPatten.alpha=0.2f;
                Right_InfoPaintSkill.alpha=0.2f;
                Right_InfoStory.enabled = false;
                Right_InfoFacialPatten.enabled = true;
                Right_InfoPaintSkill.enabled = true;
                return;
            case 1:
                Right_TextArea.text = PaintLevelManager.Instance.CurrentFacialPatten.InfoFacialPatten;
                Right_InfoStory.alpha=0.2f;
                Right_InfoFacialPatten.alpha=1f;
                Right_InfoPaintSkill.alpha=0.2f;
                Right_InfoStory.enabled = true;
                Right_InfoFacialPatten.enabled = false;
                Right_InfoPaintSkill.enabled = true;
                return;
            case 2:
                Right_TextArea.text = PaintLevelManager.Instance.CurrentFacialPatten.InfoPaintSkill;
                Right_InfoStory.alpha=0.2f;
                Right_InfoFacialPatten.alpha=0.2f;
                Right_InfoPaintSkill.alpha=1f;
                Right_InfoStory.enabled = true;
                Right_InfoFacialPatten.enabled = true;
                Right_InfoPaintSkill.enabled = false;
                return;
            default:
                return;
        }
    }

    //筛选脸谱操作
    public void OnChangeDropDownValue()
    {
        List<FacialPatten> facialPattensOnShow = new List<FacialPatten>();
        foreach (var facialPatten in FacialPattenList)
        {
            if ((Left_HangDang.value == 0 || facialPatten.HangDang == (HangDang)Left_HangDang.value)
            && (Left_MainColor.value == 0 || facialPatten.MainColor == (MainColor)Left_MainColor.value)
            && (Left_Struct.value == 0 || facialPatten.PuShi == (PuShi)Left_Struct.value))
            {
                facialPattensOnShow.Add(facialPatten);
            }
        }
        ShowFacialPattenGroup(facialPattensOnShow);
    }
    void ShowFacialPattenGroup(List<FacialPatten> facialPattenList)
    {
        for (int i = 0; i < Left_FacialGroup.transform.childCount; i++)
        {
            Destroy(Left_FacialGroup.transform.GetChild(i).gameObject);
        }
        if (facialPattenList.Count > 0)
        {
            foreach (var facialPatten in facialPattenList)
            {
                UI_FacialPattenBtn instance = Instantiate(Left_FacialPatten);
                instance.SetFacialPatternBtn(facialPatten);
                instance.transform.SetParent(Left_FacialGroup.transform);
            }

            //默认选中第一个脸谱，将信息默认设置为故事
            PaintLevelManager.Instance.SetCurrentFace(facialPattenList[0]);
            OnClickInfoSwitchButton(0);
        }
    }



}
