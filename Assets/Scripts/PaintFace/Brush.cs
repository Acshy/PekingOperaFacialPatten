using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brush
{
    public Texture2D BrushMask;
    public int Channel;
    public float Smoothness;
    public int Size;
    public float Continuity;
    public float Intensity;
    public float InkAmount;
    public BrushType BrushType;

    float ink;

    public void ResetInk()
    {
        ink = InkAmount;
    }
    public void UseInk()
    {
        ink--;
        
        ink = ink > 0 ? ink : 0;
    }
    public float GetInkPercent()
    {
        if(InkAmount<=0)
        {
            Debug.LogError("墨水量没有配置");
        }
        
        return ink/InkAmount;
    }
    public Brush(BrushScriptableObject brush,int colorChannel)
    {
        BrushMask = brush.BrushMask;
        Size = brush.Size;
        Channel = colorChannel;
        Intensity = brush.Intensity;
        InkAmount = brush.InkAmount;
        Continuity = brush.Continuity;
        BrushType = brush.BrushType;
        Smoothness= brush.Smoothness;
        ResetInk();
    }
}

