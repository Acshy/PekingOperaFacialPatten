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
    public Color[] ChannelColors = new Color[8];
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
    public Color GetMixedColor(float[] inputChannels)
    {
        Color result = Color.black;
        for (int i = 0; i < inputChannels.Length; i++)
        {
            result += ChannelColors[i] * inputChannels[i];
        }
        return result;
    }
    public void PaintOnChannel(int channel, float channelValue, ref Color mask1, ref Color mask2, out Color resultColor, out float channelSumValue)
    {
        channelSumValue = 0;
        float[] inputChannels = GetChannelsFromMask(mask1, mask2);
        inputChannels[channel] = Mathf.Clamp01(inputChannels[channel] + channelValue);
        for (int i = 0; i < 8; i++)
        {
            if (i != channel)
            {
                inputChannels[i] *= 1 - inputChannels[channel];
            }
            channelSumValue += inputChannels[i];
        }

        mask1.r = inputChannels[0];
        mask1.g = inputChannels[1];
        mask1.b = inputChannels[2];
        mask1.a = inputChannels[3];
        mask2.r = inputChannels[4];
        mask2.g = inputChannels[5];
        mask2.b = inputChannels[6];
        mask2.a = inputChannels[7];

        resultColor = GetMixedColor(mask1, mask2);
        channelSumValue = Mathf.Clamp01(channelSumValue);
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
