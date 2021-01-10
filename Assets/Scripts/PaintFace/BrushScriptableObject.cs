using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/Brush")]
public class BrushScriptableObject : ScriptableObject
{
    public Texture2D BrushMask => brushMask;
    [SerializeField] private Texture2D brushMask;
    public int Channel => channel;
    [SerializeField]private int channel;
    public int Size => size;
    [SerializeField] private int size;
    public float Continuity=>continuity;
    [SerializeField][Range(0,10)]private float continuity;
    public float Intensity=>intensity;
    [SerializeField]private float intensity;
    public float InkAmount=>inkAmount;
    [SerializeField]private float inkAmount;

}

