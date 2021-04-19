using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Events;
using Random = System.Random;

public class UI_Panel_FacialElementGroup : UI_Panel
{
    public GridLayoutGroup FacialElementGroup;
    public UI_Button_FacialElement FacialElementObj;
    public List<Button> TabButtonList;
    public Button ClearButton;
    public Button RandomButton;
    public Button FinishButton;
    
    [Header("脸谱元素列表")]
    public List<FacialElementScriptableObject> FacialElementsList_Face;
    public List<FacialElementScriptableObject> FacialElementsList_Brow;
    public List<FacialElementScriptableObject> FacialElementsList_Eye;
    public List<FacialElementScriptableObject> FacialElementsList_ForeHead;
    public List<FacialElementScriptableObject> FacialElementsList_Nose;
    public List<FacialElementScriptableObject> FacialElementsList_Mouse;
    public List<FacialElementScriptableObject> FacialElementsList_Other;

    [Header("当前所选")]
    [ShowInInspector][ReadOnly]private int currentFacialElementsID_Face=-1;
    [ShowInInspector][ReadOnly]private int currentFacialElementsID_Brow=-1;
    [ShowInInspector][ReadOnly]private int currentFacialElementsID_Eye=-1;
    [ShowInInspector][ReadOnly]private int currentFacialElementsID_ForeHead=-1;
    [ShowInInspector][ReadOnly]private int currentFacialElementsID_Nose=-1;
    [ShowInInspector][ReadOnly]private int currentFacialElementsID_Mouse=-1;
    [ShowInInspector][ReadOnly]private int currentFacialElementsID_Other=-1;
    
    // Start is called before the first frame update
    void Start()
    {
        OnClickTabButton(0);
        ClearButton.onClick.AddListener(OnClickClearButton);
        RandomButton.onClick.AddListener(OnClickRandomButton);
        FinishButton.onClick.AddListener(OnClickFinishButton);
    }

    public LimitPushi GetCurrentFacialElementLimitPuShi()
    {
        if (currentFacialElementsID_Face > 0)
        {
            return FacialElementsList_Face[currentFacialElementsID_Face].LimitPushi[0];
        }
        else
        {
            return LimitPushi.None;
            
        }

    }

    
    //功能按钮相关
    public void OnClickClearButton()
    { 
        currentFacialElementsID_Face=-1;
        currentFacialElementsID_Brow=-1;
        currentFacialElementsID_Eye=-1;
        currentFacialElementsID_ForeHead=-1;
        currentFacialElementsID_Nose=-1;
        currentFacialElementsID_Mouse=-1;
        currentFacialElementsID_Other=-1;
        OnClickTabButton(0);
        FaceTextureManager_CreateFace.Instance.InitFaceTexture();
    }
    public void OnClickRandomButton()
    {
        //写的辣鸡代码，能跑就行
        int faceRand = UnityEngine.Random.Range(0,FacialElementsList_Face.Count);
        if(faceRand<3)
            faceRand = UnityEngine.Random.Range(0,FacialElementsList_Face.Count);
        FaceTextureManager_CreateFace.Instance.SetTexture("_FacePartTexture",FacialElementsList_Face[faceRand].ElementImage);
        FaceTextureManager_CreateFace.Instance.SetTexture("_FacePartTexture2",FacialElementsList_Face[faceRand].ElementImage2);
        FaceTextureManager_CreateFace.Instance.SetColor("_FacePartColor",
            EnumTools.Inst.GetMainColor(FacialElementsList_Face[faceRand].SelectableColor[UnityEngine.Random.Range(0,FacialElementsList_Face[faceRand].SelectableColor.Count)]));
        currentFacialElementsID_Face = faceRand;
        
        List<FacialElementScriptableObject> tempList = new List<FacialElementScriptableObject>();
        foreach (var element in FacialElementsList_Brow)
        {
            if (element.LimitPushi.Contains(FacialElementsList_Face[faceRand].LimitPushi[0]))
                tempList.Add(element);
        }
        FacialElementScriptableObject facialElementScriptable = tempList[UnityEngine.Random.Range(1, tempList.Count)];
        FaceTextureManager_CreateFace.Instance.SetTexture("_BrowPartTexture",facialElementScriptable.ElementImage);
        FaceTextureManager_CreateFace.Instance.SetColor("_BrowPartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
        currentFacialElementsID_Brow = FacialElementsList_Brow.IndexOf(facialElementScriptable);
 
        tempList = new List<FacialElementScriptableObject>();
        foreach (var element in FacialElementsList_Eye)
        {
            if (element.LimitPushi.Contains(FacialElementsList_Face[faceRand].LimitPushi[0]))
                tempList.Add(element);
        } 
        facialElementScriptable = tempList[UnityEngine.Random.Range(1, tempList.Count)];
        FaceTextureManager_CreateFace.Instance.SetTexture("_EyePartTexture",facialElementScriptable.ElementImage);
        FaceTextureManager_CreateFace.Instance.SetColor("_EyePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
        currentFacialElementsID_Eye = FacialElementsList_Eye.IndexOf(facialElementScriptable);
        
        tempList = new List<FacialElementScriptableObject>();
        foreach (var element in FacialElementsList_ForeHead)
        {
            if (element.LimitPushi.Contains(FacialElementsList_Face[faceRand].LimitPushi[0]))
                tempList.Add(element);
        } 
        facialElementScriptable = tempList[UnityEngine.Random.Range(0, tempList.Count)];
        FaceTextureManager_CreateFace.Instance.SetTexture("_ForeHeadPartTexture",facialElementScriptable.ElementImage);
        FaceTextureManager_CreateFace.Instance.SetColor("_ForeHeadPartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
        currentFacialElementsID_ForeHead = FacialElementsList_ForeHead.IndexOf(facialElementScriptable);
        
        tempList = new List<FacialElementScriptableObject>();
        foreach (var element in FacialElementsList_Nose)
        {
            if (element.LimitPushi.Contains(FacialElementsList_Face[faceRand].LimitPushi[0]))
                tempList.Add(element);
        } 
        facialElementScriptable = tempList[UnityEngine.Random.Range(0, tempList.Count)];
        FaceTextureManager_CreateFace.Instance.SetTexture("_NosePartTexture",facialElementScriptable.ElementImage);
        FaceTextureManager_CreateFace.Instance.SetColor("_NosePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
        currentFacialElementsID_Nose = FacialElementsList_Nose.IndexOf(facialElementScriptable);
        
        tempList = new List<FacialElementScriptableObject>();
        foreach (var element in FacialElementsList_Mouse)
        {
            if (element.LimitPushi.Contains(FacialElementsList_Face[faceRand].LimitPushi[0]))
                tempList.Add(element);
        } 
        facialElementScriptable = tempList[UnityEngine.Random.Range(1, tempList.Count)];
        FaceTextureManager_CreateFace.Instance.SetTexture("_MousePartTexture",facialElementScriptable.ElementImage);
        FaceTextureManager_CreateFace.Instance.SetColor("_MousePartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
        currentFacialElementsID_Mouse = FacialElementsList_Mouse.IndexOf(facialElementScriptable);
        
        tempList = new List<FacialElementScriptableObject>();
        foreach (var element in FacialElementsList_Other)
        {
            if (element.LimitPushi.Contains(FacialElementsList_Face[faceRand].LimitPushi[0]))
                tempList.Add(element);
        } 
        facialElementScriptable = tempList[UnityEngine.Random.Range(1, tempList.Count)];
        FaceTextureManager_CreateFace.Instance.SetTexture("_DecoPartTexture",facialElementScriptable.ElementImage);
        FaceTextureManager_CreateFace.Instance.SetColor("_DecoPartColor",EnumTools.Inst.GetMainColor(facialElementScriptable.SelectableColor[0]));
        currentFacialElementsID_Other= FacialElementsList_Other.IndexOf(facialElementScriptable);
        OnClickTabButton(0);
    }
    public void OnClickFinishButton()
    {
    }


    //页签按钮相关
    public void OnClickTabButton(int btnId)
    {
        //设置Tab按钮激活
        SetTabButtonState(btnId);
        //刷新Tab上的脸谱元素按钮
        switch (btnId)
        {
            case 0:
                ShowFacialElementGroup(FacialElementsList_Face,currentFacialElementsID_Face);
                break;
            case 1:
                ShowFacialElementGroup(FacialElementsList_Brow,currentFacialElementsID_Brow);
                break;
            case 2:
                ShowFacialElementGroup(FacialElementsList_Eye,currentFacialElementsID_Eye);
                break;
            case 3:
                ShowFacialElementGroup(FacialElementsList_ForeHead,currentFacialElementsID_ForeHead);
                break;
            case 4:
                ShowFacialElementGroup(FacialElementsList_Nose,currentFacialElementsID_Nose);
                break;
            case 5:
                ShowFacialElementGroup(FacialElementsList_Mouse,currentFacialElementsID_Mouse);
                break;
            case 6:
                ShowFacialElementGroup(FacialElementsList_Other,currentFacialElementsID_Other);
                break;
            default:
                Debug.LogError("脸谱元素TAB按钮ID配置错误");
                break;
        }
    }
    void SetTabButtonState(int btnId)
    {
        for (int i = 0; i < TabButtonList.Count; i++)
        {
            if (i == btnId)
            {
                TabButtonList[i].interactable = false;
                TabButtonList[i].GetComponentInChildren<Text>().color=Color.white;
            }
            else
            {
                TabButtonList[i].interactable = true;
                TabButtonList[i].GetComponentInChildren<Text>().color=Color.grey;
            }
        }
    }

    
    //图形谱式按钮内容显示相关
    void ShowFacialElementGroup(List<FacialElementScriptableObject> facialElementList,int choosedBtnID)
    {
        //由于删除操作有延迟，所以生成要慢一帧
        StartCoroutine(DelayShowFacialElementGroup(facialElementList, choosedBtnID));
    }
    IEnumerator DelayShowFacialElementGroup(List<FacialElementScriptableObject> facialElementList, int choosedBtnID)
    {
        //删除旧的
        for (int i = 0; i < FacialElementGroup.transform.childCount; i++)
        {
            Destroy(FacialElementGroup.transform.GetChild(i).gameObject);
        }
        yield return null;
        //生成新的
        if (facialElementList.Count > 0)
        {
            foreach (var facialPatten in facialElementList)
            {
                var instance = Instantiate(FacialElementObj,FacialElementGroup.transform);
                instance.GetComponent<UI_Button_FacialElement>().SetFacialElementBtn(facialPatten);
                instance.gameObject.SetActive(true);
            }
        }

        //标记出已选择的
        if(choosedBtnID >= 0)
        {
            FacialElementGroup.transform.GetChild(choosedBtnID).GetComponent<UI_Button_FacialElement>().SetChoosen(true);
        }  
    }
    public void SetCurrentChoosen(FacialElementScriptableObject facialElement,int transId)
    {
        switch (facialElement.FacialElementType)
        {
            case FacialElementType.Face:
                if(currentFacialElementsID_Face >= 0)
                {
                    FacialElementGroup.transform.GetChild(currentFacialElementsID_Face)
                        .GetComponent<UI_Button_FacialElement>().SetChoosen(false);
                }
                FacialElementGroup.transform.GetChild(transId).GetComponent<UI_Button_FacialElement>().SetChoosen(true);
                currentFacialElementsID_Face = transId;
                break;
            case FacialElementType.Brow:
                if (currentFacialElementsID_Brow >= 0)
                {
                    FacialElementGroup.transform.GetChild(currentFacialElementsID_Brow)
                        .GetComponent<UI_Button_FacialElement>().SetChoosen(false);
                }
                FacialElementGroup.transform.GetChild(transId).GetComponent<UI_Button_FacialElement>().SetChoosen(true);
                currentFacialElementsID_Brow = transId;
                break;
            case FacialElementType.Eye:
                if (currentFacialElementsID_Eye >= 0)
                {
                    FacialElementGroup.transform.GetChild(currentFacialElementsID_Eye)
                        .GetComponent<UI_Button_FacialElement>().SetChoosen(false);
                }
                FacialElementGroup.transform.GetChild(transId).GetComponent<UI_Button_FacialElement>().SetChoosen(true);
                currentFacialElementsID_Eye = transId;
                break;
            case FacialElementType.ForeHead:
                if (currentFacialElementsID_ForeHead >= 0)
                {
                    FacialElementGroup.transform.GetChild(currentFacialElementsID_ForeHead)
                        .GetComponent<UI_Button_FacialElement>().SetChoosen(false);
                }
                FacialElementGroup.transform.GetChild(transId).GetComponent<UI_Button_FacialElement>().SetChoosen(true);
                currentFacialElementsID_ForeHead = transId;
                break;
            case FacialElementType.Nose:
                if (currentFacialElementsID_Nose >= 0)
                {
                    FacialElementGroup.transform.GetChild(currentFacialElementsID_Nose)
                        .GetComponent<UI_Button_FacialElement>().SetChoosen(false);
                }
                FacialElementGroup.transform.GetChild(transId).GetComponent<UI_Button_FacialElement>().SetChoosen(true);
                currentFacialElementsID_Nose = transId;
                break;
            case FacialElementType.Mouse:
                if (currentFacialElementsID_Mouse >= 0)
                {
                    FacialElementGroup.transform.GetChild(currentFacialElementsID_Mouse)
                        .GetComponent<UI_Button_FacialElement>().SetChoosen(false);
                }
                FacialElementGroup.transform.GetChild(transId).GetComponent<UI_Button_FacialElement>().SetChoosen(true);
                currentFacialElementsID_Mouse = transId;
                break;
            case FacialElementType.Other:
                if (currentFacialElementsID_Other >= 0)
                {
                    FacialElementGroup.transform.GetChild(currentFacialElementsID_Other)
                        .GetComponent<UI_Button_FacialElement>().SetChoosen(false);
                }
                FacialElementGroup.transform.GetChild(transId).GetComponent<UI_Button_FacialElement>().SetChoosen(true);
                currentFacialElementsID_Other = transId;
                break;
            default:
                break;
        }
    }
}