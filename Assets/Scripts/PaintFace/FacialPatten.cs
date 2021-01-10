using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "ScriptableObj/FacialPatten")]
public class FacialPatten : ScriptableObject
{

    public Texture Icon => icon;
    [FoldoutGroup("基本信息")] [LabelText("预览图")] [PreviewField(100, ObjectFieldAlignment.Left)] [SerializeField] private Texture icon;
    public string FaceName => faceName;
    [FoldoutGroup("基本信息")] [LabelText("名称")] [SerializeField] private string faceName;
    public string Belong => belong;
    [FoldoutGroup("基本信息")] [LabelText("出自")] [SerializeField] private string belong;
    public string Introduction => introduction;
    [FoldoutGroup("基本信息")] [LabelText("介绍")] [SerializeField] [Multiline] private string introduction;
    public HangDang HangDang => handDang;
    [FoldoutGroup("基本信息")] [LabelText("行当")] [SerializeField] private HangDang handDang;
    public PuShi PuShi => puShi;
    [FoldoutGroup("基本信息")] [LabelText("谱式")] [SerializeField] private PuShi puShi;



    public PaintDifficulty PaintDifficulty => paintDifficulty;
    [FoldoutGroup("绘制信息")] [LabelText("脸谱难度")] [SerializeField] private PaintDifficulty paintDifficulty;
    public List<Color> ExtraColors => extraColors;
    [FoldoutGroup("绘制信息")] [LabelText("脸谱调色板")] [ColorPalette("FacialColor")] [SerializeField] private List<Color> extraColors = new List<Color>(4);
    public Texture MaskMap => maskMap;
    [FoldoutGroup("绘制信息")] [LabelText("绘制区域Mask")] [PreviewField(100, ObjectFieldAlignment.Left)] [SerializeField] private Texture maskMap;


}

[System.Serializable]
public enum HangDang
{
    [LabelText("生")] Sheng = 0,
    [LabelText("旦")] Dan = 1,
    [LabelText("净")] Jin = 2,
    [LabelText("丑")] Chou = 3
}
[System.Serializable]
public enum PuShi
{
    [LabelText("丑角脸")] ChouJueLian = -2,
    [LabelText("俊扮")] JunBan = -1,
    [LabelText("整脸")] ZhengLian = 0,
    [LabelText("三块瓦")] SanKuaiWa = 1,
    [LabelText("十字门脸")] ShiZiMengLian = 2,
    [LabelText("六分脸")] LiuFengLian = 3,
    [LabelText("碎花脸")] SuiHuaLian = 4,
    [LabelText("元宝脸")] YuanbaoLian = 5,
    [LabelText("歪脸")] WaiLian = 6,
    [LabelText("象形脸")] XiangXingLian = 7,
    [LabelText("神仙脸")] ShengXianLian = 8,
    [LabelText("小妖脸")] XiaoYaoLian = 9,
    [LabelText("英雄脸")] YingXiongLian = 10,
    [LabelText("僧道脸")] SengDaoLian = 11,
}
[System.Serializable]
public enum PaintDifficulty
{
    Easy = 0,
    Normal = 1,
    Hard = 2
}
