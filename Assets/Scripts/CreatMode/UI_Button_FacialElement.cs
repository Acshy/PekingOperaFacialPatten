using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_FacialElement : MonoBehaviour
{
    [HideInInspector]public FacialElementScriptableObject facialElementScriptable;
    public bool IsChoosen = false;
    public UI_Panel_FacialElementGroup FacialElementPanel;
    
    public Image ElementImage;
    public Image ElementImage2;
    public Text NameText;
    public Text LimitText;
    public Transform ColorButtonGroup;
    public Button ColorButton;

    public void SetFacialElementBtn(FacialElementScriptableObject _facialElement)
    {
        //设置脸谱元素的基本信息
        facialElementScriptable = _facialElement;
        if(_facialElement.ElementImage!=null)
        {
            ElementImage.sprite = _facialElement.ElementImage;
            ElementImage.color = Color.Lerp(EnumTools.Inst.GetMainColor(_facialElement.SelectableColor[0]),Color.white*0.8f,0.2f);
        }
        else
        {
            ElementImage.color = new Color(0, 0, 0, 0);
        } 
        if(_facialElement.ElementImage2!=null)
        {
            ElementImage2.sprite = _facialElement.ElementImage2;
            ElementImage2.color = Color.white;
        }
        else
        {
            ElementImage2.color = new Color(0, 0, 0, 0);
        }
        NameText.text = _facialElement.ElementName;
        
        //创建并设置颜色按钮
        for (int i = 0; i < _facialElement.SelectableColor.Count; i++)
        {
            var instance = Instantiate(ColorButton, ColorButtonGroup);
            instance.transform.GetChild(0).GetComponent<Image>().color = EnumTools.Inst.GetMainColor(_facialElement.SelectableColor[i]);
            if (_facialElement.SelectableColor[i] == MainColor.White)
            {
                instance.GetComponent<Image>().color=Color.grey;
            }
            instance.GetComponent<Button>().onClick.AddListener(
                delegate {
                    OnClickColor(instance.transform.GetSiblingIndex());
                });
            instance.gameObject.SetActive(true);
        }
        LimitText.text = "";
        ColorButtonGroup.gameObject.SetActive(false);

        //设置是否可选
        if (_facialElement.FacialElementType!=FacialElementType.Face)
        {
            bool canChoose = false;
            for (int i = 0; i < _facialElement.LimitPushi.Count; i++)
            {
                if (FacialElementPanel.GetCurrentFacialElementLimitPuShi() == _facialElement.LimitPushi[i])
                {
                    canChoose = true;
                }
            }
            if (!canChoose)
            {
                LimitText.text = "（不适用当前脸底）";
                LimitText.color = Color.grey;
                NameText.color = Color.grey;
                GetComponent<Button>().interactable = false;
            }
        }
    }
    

    public void SetChoosen(bool isChoosen)
    {
        IsChoosen = isChoosen;
        if (isChoosen)
        {
            ColorButtonGroup.gameObject.SetActive(true);
            GetComponent<Button>().interactable = false;
        }
        else
        {
            ColorButtonGroup.gameObject.SetActive(false);
            GetComponent<Button>().interactable = true;
        }
    }

    public void OnClickFacialElement()
    {
        //传递ID，设置当前所选
        FacialElementPanel.SetCurrentChoosen(facialElementScriptable,transform.GetSiblingIndex());
        //设置图片和颜色
        switch (facialElementScriptable.FacialElementType)
        {
            case FacialElementType.Face:
                FaceTextureManager_CreateFace.Instance.SetTexture("_FacePartTexture",facialElementScriptable.ElementImage);
                FaceTextureManager_CreateFace.Instance.SetTexture("_FacePartTexture2",facialElementScriptable.ElementImage2);
                FaceTextureManager_CreateFace.Instance.SetColor("_FacePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
                break;
            case FacialElementType.Brow:
                FaceTextureManager_CreateFace.Instance.SetTexture("_BrowPartTexture",facialElementScriptable.ElementImage);
                FaceTextureManager_CreateFace.Instance.SetColor("_BrowPartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
                break;
            case FacialElementType.Eye:
                FaceTextureManager_CreateFace.Instance.SetTexture("_EyePartTexture",facialElementScriptable.ElementImage);
                FaceTextureManager_CreateFace.Instance.SetColor("_EyePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
                break;
            case FacialElementType.ForeHead:
                FaceTextureManager_CreateFace.Instance.SetTexture("_ForeHeadPartTexture",facialElementScriptable.ElementImage);
                FaceTextureManager_CreateFace.Instance.SetColor("_ForeHeadPartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
                break;
            case FacialElementType.Nose:
                FaceTextureManager_CreateFace.Instance.SetTexture("_NosePartTexture",facialElementScriptable.ElementImage);
                FaceTextureManager_CreateFace.Instance.SetColor("_NosePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
                break;
            case FacialElementType.Mouse:
                FaceTextureManager_CreateFace.Instance.SetTexture("_MousePartTexture",facialElementScriptable.ElementImage);
                FaceTextureManager_CreateFace.Instance.SetColor("_MousePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
                break;
            case FacialElementType.Other:
                FaceTextureManager_CreateFace.Instance.SetTexture("_DecoPartTexture",facialElementScriptable.ElementImage);
                FaceTextureManager_CreateFace.Instance.SetColor("_DecoPartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnClickColor(int index)
    {
        switch (facialElementScriptable.FacialElementType)
        {
            case FacialElementType.Face:
                FaceTextureManager_CreateFace.Instance.SetColor("_FacePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[index]));
                break;
            case FacialElementType.Brow:
                FaceTextureManager_CreateFace.Instance.SetColor("_BrowPartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[index]));
                break;
            case FacialElementType.Eye:
                FaceTextureManager_CreateFace.Instance.SetColor("_EyePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[index]));
                break;
            case FacialElementType.ForeHead:
                FaceTextureManager_CreateFace.Instance.SetColor("_ForeHeadPartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[index]));
                break;
            case FacialElementType.Nose:
                FaceTextureManager_CreateFace.Instance.SetColor("_NosePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[index]));
                break;
            case FacialElementType.Mouse:
                FaceTextureManager_CreateFace.Instance.SetColor("_MousePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[index]));
                break;
            case FacialElementType.Other:
                FaceTextureManager_CreateFace.Instance.SetColor("_DecoPartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[index]));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
