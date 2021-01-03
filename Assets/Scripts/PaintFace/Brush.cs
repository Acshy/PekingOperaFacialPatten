using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObj/Brush")]
public class Brush : ScriptableObject
{
    public Texture2D BrushMask;
    public int Channel;
    public int Size;    
    public float Intensity;
    public float MaxDistance;

    public Brush(Texture2D _texture, int _size, int _channel, float _intensity, float _maxDistance)
    {
        BrushMask = _texture;
        Size = _size;
        Channel = _channel;
        Intensity = _intensity;
        MaxDistance = _maxDistance;
    }
}

