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
        FacialPattenTexture = AssetDatabase.LoadAssetAtPath<Texture>(textureImagePath + "Texture_FacialPattenTexture_" + obj.FileName + ".png");
        MaskMap1 = AssetDatabase.LoadAssetAtPath<Texture>(maskImagePath + "Texture_FacialPattenTexture_" + obj.FileName + "_mask1.png");
        MaskMap2 = AssetDatabase.LoadAssetAtPath<Texture>(maskImagePath + "Texture_FacialPattenTexture_" + obj.FileName + "_mask2.png");
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
    [HideLabel] [PreviewField(100, ObjectFieldAlignment.Left)] [HorizontalGroup("row1", 120)] [SerializeField] public Texture FacialPattenTexture;
    [PropertySpace(SpaceBefore = 30, SpaceAfter = 30)]
    [HideLabel] [PreviewField(60, ObjectFieldAlignment.Right)] [HorizontalGroup("row1", 60)] [SerializeField] public Texture MaskMap1;
    [PropertySpace(SpaceBefore = 30, SpaceAfter = 30)]
    [HideLabel] [PreviewField(60, ObjectFieldAlignment.Right)] [HorizontalGroup("row1", 60)] [SerializeField] public Texture MaskMap2;

    [FoldoutGroup("基本信息")] [LabelText("文件名称")] [SerializeField] public string FileName;
    [FoldoutGroup("基本信息")] [LabelText("名称")] [SerializeField] public string FaceName;
    [FoldoutGroup("基本信息")] [LabelText("出自")] [SerializeField] public string From;
    [FoldoutGroup("基本信息")] [LabelText("行当")] [SerializeField] public HangDang HangDang;
    [FoldoutGroup("基本信息")] [LabelText("颜色")] [SerializeField] public MainColor MainColor;
    [FoldoutGroup("基本信息")] [LabelText("谱式")] [SerializeField] public PuShi PuShi;

    [FoldoutGroup("基本信息")] [LabelText("脸谱解读")] [SerializeField] [Multiline] public string InfoFacialPatten;
    [FoldoutGroup("基本信息")] [LabelText("角色介绍")] [SerializeField] [Multiline] public string InfoCharacter;
    [FoldoutGroup("基本信息")] [LabelText("戏曲故事")] [SerializeField] [Multiline] public string InfoStory;

}

[System.Serializable]
public enum HangDang
{
    None = 0,
    [LabelText("生")] Sheng = 1,
    [LabelText("旦")] Dan = 2,
    [LabelText("净")] Jing = 3,
    [LabelText("丑")] Chou = 4
}
[System.Serializable]
public enum PuShi
{
    None = 0,
    [LabelText("整脸")] ZhengLian = 1,
    [LabelText("三块瓦")] SanKuaiWa = 2,
    [LabelText("十字门脸")] ShiZiMengLian = 3,
    [LabelText("六分脸")] LiuFengLian = 4,
    [LabelText("碎花脸")] SuiHuaLian = 5,
    [LabelText("元宝脸")] YuanBaoLian = 6,
    [LabelText("歪脸")] WaiLian = 7,
    [LabelText("象形脸")] XiangXingLian = 8,
    [LabelText("俊扮")] JunBan = 9,
    [LabelText("其它")] Other = 10,
    // [LabelText("神仙脸")] ShengXianLian = 9,
    // [LabelText("小妖脸")] XiaoYaoLian = 10,
    // [LabelText("英雄脸")] YingXiongLian = 11,
    // [LabelText("僧道脸")] SengDaoLian = 12,
    // [LabelText("丑角脸")] ChouJueLian = 13,
}

[System.Serializable]
public enum MainColor
{
    None = 0,
    Red = 1,
    Purple = 2,
    Black = 3,
    White = 4,
    Blue = 5,
    Green = 6,
    Yellow = 7,
    Pink = 8,
    Goldn = 9,
    Silver = 10
}
