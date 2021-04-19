using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class EnumTools : TSingleton<EnumTools>
{
    public string GetPuShiString(PuShi puShi)
    {
        switch (puShi)
        {
            case PuShi.ZhengLian:
                return "整脸";
            case PuShi.SanKuaiWa:
                return "三块瓦";
            case PuShi.ShiZiMengLian:
                return "十字门脸";
            case PuShi.LiuFenLian:
                return "六分脸";
            case PuShi.SuiHuaLian:
                return "整脸";
            case PuShi.YuanBaoLian:
                return "元宝脸";
            case PuShi.Chou:
                return "丑脸";
            case PuShi.XiangXingLian:
                return "象形脸";
            case PuShi.JunBan:
                return "俊扮";
            case PuShi.Other:
                return "";
            default:
                return "";
        }
    }

    public string GetHangDangString(HangDang hangDang)
    {
        switch (hangDang)
        {
            case HangDang.Sheng:
                return "生行";
            case HangDang.Dan:
                return "旦行";
            case HangDang.Jing:
                return "净行";
            case HangDang.Chou:
                return "丑行";
            default:
                return "";
        }
    }

    public Color GetMainColor(MainColor mainColor)
    {
        switch (mainColor)
        {
            case MainColor.None:
                return new Color(1, 0, 1, 1);
            case MainColor.Red:
                return new Color(1, 0, 0, 1);
            case MainColor.Purple:
                return new Color(0.55f, 0, 0.2f, 1);
            case MainColor.Black:
                return new Color(0, 0, 0, 1);
            case MainColor.White:
                return new Color(1, 1, 1, 1);
            case MainColor.Blue:
                return new Color(0.1f, 0.45f, 0.9f, 1);
            case MainColor.Green:
                return new Color(0.1f, 0.5f, 0.1f, 1);
            case MainColor.Yellow:
                return new Color(1, 0.8f, 0, 1);
            case MainColor.Pink:
                return new Color(1, 0.5f, 0.4f, 1);
            case MainColor.Goldn:
                return new Color(1, 0, 0, 1);
            case MainColor.Silver:
                return new Color(0.6f, 0.6f, 0.6f, 1);
            default:
                return new Color(1, 0, 1, 1);
        }
    }
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
    [LabelText("六分脸")] LiuFenLian = 4,
    [LabelText("碎花脸")] SuiHuaLian = 5,
    [LabelText("元宝脸")] YuanBaoLian = 6,
    [LabelText("丑脸")] Chou = 7,
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
