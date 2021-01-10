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
        ChannelColors[7] = Color.black;//第八个通道用来抹油，不上色
        channelColorsMat = new Matrix4x4(ChannelColors[4], ChannelColors[5], ChannelColors[6], ChannelColors[7]);
        Shader.SetGlobalMatrix("_MaskColor2", channelColorsMat);
    }
    private void OnValidate()
    {
        SetGlobColors();
    }

}
