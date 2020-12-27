using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteManager : MonoBehaviour
{
    public static PaletteManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    
    public Color[] ChannelColors = new Color[7];
    private void Start()
    {
        SetGlobColors();
    }
    public void SetGlobColors()
    {
        Matrix4x4 channelColorsMat = new Matrix4x4(ChannelColors[0], ChannelColors[1], ChannelColors[2], ChannelColors[3]);
        Shader.SetGlobalMatrix("_MaskColor1", channelColorsMat);
        channelColorsMat = new Matrix4x4(ChannelColors[4], ChannelColors[5], ChannelColors[6], Color.black);
        Shader.SetGlobalMatrix("_MaskColor2", channelColorsMat);
    }
  

}
