using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Panel_InfoPanel : UI_Panel
{
    public Image FacialImage;
    public Text NameText;
    public Text PuShiText;
    public Text FromText;
    public Text CharacterText;
    public Text StoryText;
    public Text FacialPattenText;
    [Space]
    public Button BackBtn;
    public Button ChooseBtn;
    [Space] 
    public UI_Panel FacialGroupPanel;

    void Start()
    {
        BackBtn.onClick.AddListener(OnClickBackBtn);
        ChooseBtn.onClick.AddListener(OnClickChooseBtn);
    }

    public void SetFacialPatten(FacialPattenScriptableObject facialPatten)
    {
        FacialImage.sprite = facialPatten.PreviewImage;
        NameText.text = facialPatten.FaceName;
        PuShiText.text = EnumTools.Inst.GetPuShiString(facialPatten.PuShi); //facialPatten.GetHangDangString()+" · "+ facialPatten.GetPuShiString();
        FromText.text = "出自 : " + facialPatten.From;
        CharacterText.text = facialPatten.InfoCharacter;
        StoryText.text = facialPatten.InfoStory;
        FacialPattenText.text = facialPatten.InfoFacialPatten;
    }

    public void OnClickBackBtn()
    {
        FacialGroupPanel.Show();
        FaceTextureManager.Instance.SetFacialPattenTexture(null,null,null);
        Hide();
    }

    public void OnClickChooseBtn()
    {
        CameraController.Instance.ApplyCameraData(0,1);
        FaceTextureManager.Instance.SetFacialPattenTexture(null,null,null);
        FaceTextureManager.Instance.Initialized();
        Hide();
    }
}
