using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "ScriptableObj/FacialPatten")]
public class FacialPatten : ScriptableObject
{

    public Sprite Image => image;
    [FoldoutGroup("基本信息")] [LabelText("预览图")] [PreviewField(100, ObjectFieldAlignment.Left)] [SerializeField] private Sprite image;
    public string FaceName => faceName;
    [FoldoutGroup("基本信息")] [LabelText("名称")] [SerializeField] private string faceName;
    public string From => from;
    [FoldoutGroup("基本信息")] [LabelText("出自")] [SerializeField] private string from;
    public HangDang HangDang => handDang;
    [FoldoutGroup("基本信息")] [LabelText("行当")] [SerializeField] private HangDang handDang;
    public PuShi PuShi => puShi;
    public MainColor MainColor => mainColor;
    [FoldoutGroup("基本信息")] [LabelText("颜色")] [SerializeField] private MainColor mainColor;
    [FoldoutGroup("基本信息")] [LabelText("谱式")] [SerializeField] private PuShi puShi;

    public string InfoStory => infoStory;
    [FoldoutGroup("基本信息")] [LabelText("角色介绍")] [SerializeField] [Multiline] private string infoStory;
    public string InfoFacialPatten => infoFacialPatten;
    [FoldoutGroup("基本信息")] [LabelText("脸谱解读")] [SerializeField] [Multiline] private string infoFacialPatten;
    public string InfoPaintSkill => infoPaintSkill;
    [FoldoutGroup("基本信息")] [LabelText("绘制技巧")] [SerializeField] [Multiline] private string infoPaintSkill;




    public PaintDifficulty PaintDifficulty => paintDifficulty;
    [FoldoutGroup("绘制信息")] [LabelText("脸谱难度")] [SerializeField] private PaintDifficulty paintDifficulty;
    public Color[] Palette => palette;
    [FoldoutGroup("绘制信息")] [LabelText("脸谱调色板")] [ColorPalette("FacialColor")] [SerializeField] private Color[] palette=new Color[7];
    public Texture MaskMap => maskMap;
    [FoldoutGroup("绘制信息")] [LabelText("绘制区域Mask")] [PreviewField(100, ObjectFieldAlignment.Left)] [SerializeField] private Texture maskMap;


}

[System.Serializable]
public enum HangDang
{
    None = 0,
    [LabelText("生")] Sheng = 1,
    [LabelText("旦")] Dan = 2,
    [LabelText("净")] Jin = 3,
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
    [LabelText("元宝脸")] YuanbaoLian = 6,
    [LabelText("歪脸")] WaiLian = 7,
    [LabelText("象形脸")] XiangXingLian = 8,
    [LabelText("俊扮")] JunBan = 9,

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

[System.Serializable]
public enum PaintDifficulty
{
    None = 0,
    Easy = 1,
    Normal = 2,
    Hard = 3
}
