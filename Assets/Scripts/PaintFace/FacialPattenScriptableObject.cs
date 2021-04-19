using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "ScriptableObj/FacialPatten")]
public class FacialPattenScriptableObject : ScriptableObject
{
   
    
    public void CopyValue(FacialPattenScriptableObject obj,
        string iconImagePath,
        string textureImagePath,
        string maskImagePath)
    {
        PreviewImage = AssetDatabase.LoadAssetAtPath<Sprite>(iconImagePath + "Sprite_FacialPattenPreview_" + obj.FileName + ".png");
        FacialPattenTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(textureImagePath + "Texture_FacialPattenTexture_" + obj.FileName + ".png");
        MaskMap1 = AssetDatabase.LoadAssetAtPath<Texture2D>(maskImagePath + "Texture_FacialPattenTexture_" + obj.FileName + "_mask1.png");
        MaskMap2 = AssetDatabase.LoadAssetAtPath<Texture2D>(maskImagePath + "Texture_FacialPattenTexture_" + obj.FileName + "_mask2.png");
        FileName = obj.FileName;
        FaceName = obj.FaceName;
        From = obj.From;
        HangDang = obj.HangDang;
        MainColor = obj.MainColor;
        PuShi = obj.PuShi;
        InfoFacialPatten = obj.InfoFacialPatten;
        InfoCharacter = obj.InfoCharacter;
        InfoStory = obj.InfoStory;
    }

    [PropertySpace(SpaceBefore = 30, SpaceAfter = 30)]
    [HideLabel] [PreviewField(100, ObjectFieldAlignment.Left)] [HorizontalGroup("row1", 120)] [SerializeField] public Sprite PreviewImage;
    [PropertySpace(SpaceBefore = 30, SpaceAfter = 30)]
    [HideLabel] [PreviewField(100, ObjectFieldAlignment.Left)] [HorizontalGroup("row1", 120)] [SerializeField] public Texture2D FacialPattenTexture;
    [PropertySpace(SpaceBefore = 30, SpaceAfter = 30)]
    [HideLabel] [PreviewField(60, ObjectFieldAlignment.Right)] [HorizontalGroup("row1", 60)] [SerializeField] public Texture2D MaskMap1;
    [PropertySpace(SpaceBefore = 30, SpaceAfter = 30)]
    [HideLabel] [PreviewField(60, ObjectFieldAlignment.Right)] [HorizontalGroup("row1", 60)] [SerializeField] public Texture2D MaskMap2;

    [FoldoutGroup("基本信息")] [LabelText("文件名称")] [SerializeField] public string FileName;
    [FoldoutGroup("基本信息")] [LabelText("名称")] [SerializeField] public string FaceName;
    [FoldoutGroup("基本信息")] [LabelText("出自")] [SerializeField] public string From;
    [FoldoutGroup("基本信息")] [LabelText("行当")] [SerializeField] public HangDang HangDang;
    [FoldoutGroup("基本信息")] [LabelText("颜色")] [SerializeField] public MainColor MainColor;
    [FoldoutGroup("基本信息")] [LabelText("谱式")] [SerializeField] public PuShi PuShi;

    [FoldoutGroup("基本信息")] [LabelText("脸谱解读")] [SerializeField] [Multiline] public string InfoFacialPatten;
    [FoldoutGroup("基本信息")] [LabelText("角色介绍")] [SerializeField] [Multiline] public string InfoCharacter;
    [FoldoutGroup("基本信息")] [LabelText("戏曲故事")] [SerializeField] [Multiline] public string InfoStory;
    
    [LabelText("绘制步骤")] [SerializeField]  public List<paintStep> PaintSteps;

   
}
[Serializable]
public struct  paintStep
{
    [Multiline] public string description;
    public List<MainColor> UseColor;
}