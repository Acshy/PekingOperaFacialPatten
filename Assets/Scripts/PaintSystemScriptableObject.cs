using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObj/PaintSystemAssest")]
public class PaintSystemScriptableObject : ScriptableObject
{
    
}



//用于定义颜色通道
public enum ColorType
{
    Black=0,
    White=1,
    Red=2,
    Pink=3,
    Yellow=4,
    Green=5,
    Blue=6,
    Grey=7
}
