using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Events;

public class UI_Panel_FacialGroup : UI_Panel
{
    public GridLayoutGroup FacialGroup;
    public UI_Button_FacialPatten FacialPattenObj;
    public Dropdown HangDangDD;
    public Dropdown MainColorDD;
    public Dropdown PuShiDD;

    public List<FacialPattenScriptableObject> FacialPattenList;
    // Start is called before the first frame update
    void Start()
    {
        ShowFacialPattenGroup(FacialPattenList);
        HangDangDD.onValueChanged.AddListener(onChangeDropDownValueAction());
        MainColorDD.onValueChanged.AddListener(onChangeDropDownValueAction());
        PuShiDD.onValueChanged.AddListener(onChangeDropDownValueAction());
    }

    private UnityAction<int> onChangeDropDownValueAction()
    {
        return delegate(int index)
        {
            OnChangeDropDownValue();
        };
    }


    //筛选脸谱操作
    public void OnChangeDropDownValue()
    {
        List<FacialPattenScriptableObject> facialPattensOnShow = new List<FacialPattenScriptableObject>();
        foreach (var facialPatten in FacialPattenList)
        {
            if ((HangDangDD.value == 0 || facialPatten.HangDang == (HangDang)HangDangDD.value)
            && (MainColorDD.value == 0 || facialPatten.MainColor == (MainColor)MainColorDD.value)
            && (PuShiDD.value == 0 || facialPatten.PuShi == (PuShi)PuShiDD.value))
            {
                facialPattensOnShow.Add(facialPatten);
            }
        }
        ShowFacialPattenGroup(facialPattensOnShow);
    }
    
    void ShowFacialPattenGroup(List<FacialPattenScriptableObject> facialPattenList)
    {
        for (int i = 0; i < FacialGroup.transform.childCount; i++)
        {
            Destroy(FacialGroup.transform.GetChild(i).gameObject);
        }
        if (facialPattenList.Count > 0)
        {
            foreach (var facialPatten in facialPattenList)
            {
                var instance = Instantiate(FacialPattenObj,FacialGroup.transform);
                instance.GetComponent<UI_Button_FacialPatten>().SetFacialPatternBtn(facialPatten);
            }

            //默认选中第一个脸谱
            PaintLevelManager.Instance.SetCurrentFace(null);
        }
    }



}
