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

    public int CurrentChannel { get; set; }
    public Color CurrentColor
    {
        get
        {
            return ChannelColors[CurrentChannel];
        }
    }
    public Color[] ChannelColors = new Color[7];
    private void Start()
    {
        Matrix4x4 channelColorsMat = new Matrix4x4(ChannelColors[0], ChannelColors[1], ChannelColors[2], ChannelColors[3]);
        Shader.SetGlobalMatrix("_MaskColor1", channelColorsMat);
        channelColorsMat = new Matrix4x4(ChannelColors[4], ChannelColors[5], ChannelColors[6], Color.black);
        Shader.SetGlobalMatrix("_MaskColor2", channelColorsMat);
    }

    public Color GetMixedColor(float[] inputChannels)
    {
        Color result = Color.black;
        for (int i = 0; i < inputChannels.Length; i++)
        {
            result += ChannelColors[i] * inputChannels[i];
        }
        return result;
    }

    float[] GetChannelsFromMask(Color mask1, Color mask2)
    {
        float[] result = new float[8];
        result[0] = mask1.r;
        result[1] = mask1.g;
        result[2] = mask1.b;
        result[3] = mask1.a;
        result[4] = mask2.r;
        result[5] = mask2.g;
        result[6] = mask2.b;
        result[7] = mask2.a;

        return result;
    }
    public void PaintOnChannel(int channel, float channelValue, ref float[] inputChannels)
    {
        inputChannels[channel] = Mathf.Clamp01(inputChannels[channel] + channelValue);
        for (int i = 0; i < 8; i++)
        {
            if (i != channel)
            {
                inputChannels[i] *= 1 - inputChannels[channel];
            }
        }
    }
    public void PaintOnChannel(int channel, float channelValue, ref Color mask1, ref Color mask2)
    {
        mask1.r = Mathf.Lerp(mask1.r, channel == 0 ? 1 : 0, channelValue);
        mask1.g = Mathf.Lerp(mask1.g, channel == 1 ? 1 : 0, channelValue);
        mask1.b = Mathf.Lerp(mask1.b, channel == 2 ? 1 : 0, channelValue);
        mask1.a = Mathf.Lerp(mask1.a, channel == 3 ? 1 : 0, channelValue);
        mask2.r = Mathf.Lerp(mask2.r, channel == 4 ? 1 : 0, channelValue);
        mask2.g = Mathf.Lerp(mask2.g, channel == 5 ? 1 : 0, channelValue);
        mask2.b = Mathf.Lerp(mask2.b, channel == 6 ? 1 : 0, channelValue);

        mask2.a = mask1.r + mask1.g + mask1.b + mask1.a + mask2.r + mask2.g + mask2.b;
    }
    public Color GetMixedColor(Color mask1, Color mask2)
    {
        float[] inputChannels = GetChannelsFromMask(mask1, mask2);
        Color result = Color.black;
        for (int i = 0; i < inputChannels.Length; i++)
        {
            result += ChannelColors[i] * inputChannels[i];
        }
        return result;
    }
}
