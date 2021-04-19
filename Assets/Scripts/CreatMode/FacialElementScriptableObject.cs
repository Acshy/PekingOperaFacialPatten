using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "ScriptableObj/FacialElement")]
public class FacialElementScriptableObject : ScriptableObject
{
    [LabelText("元素图")][PreviewField(100)] [SerializeField] public Sprite ElementImage;
    [LabelText("面部附图")][PreviewField(100)][SerializeField] public Sprite ElementImage2;
    [LabelText("元素名")] [SerializeField] public string ElementName;
    [LabelText("元素介绍")] [SerializeField] [Multiline] public string ElementDes;
    [LabelText("元素类型")] [SerializeField] public FacialElementType FacialElementType;
    
    [LabelText("谱式限制")] [SerializeField] public List<LimitPushi> LimitPushi=new List<LimitPushi>(new LimitPushi[0]);
    [LabelText("颜色")] [SerializeField] public List<MainColor> SelectableColor=new List<MainColor>(new MainColor[4]);
}

public enum LimitPushi
{
    None,
    SanKuaiWa,
    ZhengLian,
    LiuFengLian,
    ChouJueLian
}

public enum FacialElementType
{
    Face,
    Brow,
    Eye,
    ForeHead,
    Nose,
    Mouse,
    Other
}