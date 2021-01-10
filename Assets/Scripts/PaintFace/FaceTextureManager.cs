using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FaceTextureManager : MonoBehaviour
{

    public static FaceTextureManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public Renderer FaceRenderer;
    Texture2D baseMap;
    //Texture2D resultBaseMap;
    Texture2D controlMask1;
    Texture2D controlMask2;
    //float[,][] controlMask;

    public float Correction;
    public Texture2D PaintAreaTexture1;
    public Texture2D PaintAreaTexture2;

    private void Start()
    {
        Initialized(FaceRenderer);
        PaintActionQueue = new List<IEnumerator>();
    }

    private void Initialized(Renderer renderer)
    {
        //获取颜色贴图
        if (renderer)
            baseMap = (Texture2D)renderer.sharedMaterial.GetTexture("_BaseMap");


        //初始化结果图
        //resultBaseMap = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true);
        //controlMask = new float[baseMap.width, baseMap.height][];
        controlMask1 = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true, true);
        controlMask2 = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true, true);
        for (int x = 0; x < baseMap.width; x++)
        {
            for (int y = 0; y < baseMap.height; y++)
            {
                //resultBaseMap.SetPixel(x, y, baseMap.GetPixel(x, y));
                //controlMask[x, y] = new float[PaletteManager.Instance.ChannelColors.Length];
                controlMask1.SetPixel(x, y, new Color(0, 0, 0, 0));
                controlMask2.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        //resultBaseMap.Apply();
        controlMask1.Apply();
        controlMask2.Apply();
        //renderer.material.SetTexture("_BaseMap", resultBaseMap);
        renderer.material.SetTexture("_ControlMask1", controlMask1);
        renderer.material.SetTexture("_ControlMask2", controlMask2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveRenderTextureToPNG(controlMask1, "TextureSaved_Mask1");
        }
        //每一帧从绘制队列中取值绘制
        if (PaintActionQueue.Count>0)
        {
            StartCoroutine(PaintActionQueue[0]);
            PaintActionQueue.RemoveAt(0);
        }
    }
    public void Paint(Vector2 currentUV, Vector2 lastUV, Brush brush, PointType pointType)
    {
        if (pointType == PointType.StartPoint)
        {
            brush.ResetInk();
        }

        Vector2 vec2 = lastUV - currentUV;
        int pointCount=1;
        if(Vector2.SqrMagnitude(vec2) < 0.25f)
        {
            pointCount = (int)(Vector2.Distance(new Vector2(vec2.x * baseMap.width, vec2.y * baseMap.height),Vector2.zero) / brush.Size * brush.Continuity);        
            pointCount = pointCount < 1 ? 1 : pointCount;
        }
        
        Vector2 dir = (lastUV - currentUV).normalized;
        float stepDis = Vector2.Distance(lastUV, currentUV) / pointCount;

        for (int i = 0; i < pointCount; i++)
        {
            PaintActionQueue.Add(DelayPaintColor(lastUV - dir * stepDis * i, brush));
        }

        
        //StartCoroutine(PaintColorBetween(pointCount, currentUV, lastUV, brush));//使用协程优化插值

        // if (pointCount <= 1 || Vector2.SqrMagnitude(vec2) > 0.25f)
        // {
        //     PaintColor(currentUV, brush);
        // }
        // else
        // {
        //     StartCoroutine(PaintColorBetween(pointCount, currentUV, lastUV, brush));//使用协程优化插值
        // }
    }

    List<IEnumerator> PaintActionQueue;
    IEnumerator DelayPaintColor(Vector2 pointUV, Brush brush)
    {
        PaintColor(pointUV, brush);
        yield return null;
    }

    //直接使用协程插值补间，会造成后绘制的点比补间点先绘制完成
    // IEnumerator PaintColorBetween(int pointCount, Vector2 currentUV, Vector2 lastUV, Brush brush)
    // {

    //     pointCount = pointCount < 1 ? 1 : pointCount;
    //     Vector2 dir = (lastUV - currentUV).normalized;
    //     float stepDis = Vector2.Distance(lastUV, currentUV) / pointCount;

    //     for (int i = 0; i < pointCount; i++)
    //     {
    //         PaintColor(lastUV - dir * stepDis * i, brush);
    //         yield return null;

    //     }
    // }

    public void PaintColor(Vector2 pointUV, Brush brush)
    {
        if (baseMap == null)
            return;

        float intensity;
        float inkRemain = brush.GetInkPercent();

        int x, y;//笔刷贴图在模型贴图上的起始点位置
        int brushRangeWidth, brushRangeHeight;//笔刷的处理后尺寸
        int brushOffsetX, brushOffestY;//

        //通过中心点uv计算贴图起点xy
        pointUV = new Vector2(Mathf.Clamp01(pointUV.x), Mathf.Clamp01(pointUV.y));//归一化UV
        x = Mathf.FloorToInt(pointUV.x * (float)baseMap.width - ((float)brush.Size / 2));
        y = Mathf.FloorToInt(pointUV.y * (float)baseMap.height - ((float)brush.Size / 2));

        //计算笔刷真实覆盖范围的宽和高（主要处理边界的情况）
        brushRangeWidth = Mathf.Min(brush.Size, (baseMap.width - x), x + brush.Size);
        brushRangeHeight = Mathf.Min(brush.Size, (baseMap.height - y), y + brush.Size);

        //笔刷起点偏移
        brushOffsetX = x < 0 ? brush.Size - brushRangeWidth : 0;
        brushOffestY = y < 0 ? brush.Size - brushRangeHeight : 0;

        //把xy给规范到贴图范围内
        x = Mathf.Clamp(x, 0, baseMap.width - brush.Size / 2 - 1);
        y = Mathf.Clamp(y, 0, baseMap.height - brush.Size / 2 - 1);

        //获取模型贴图像素值
        Color[] mask1Color = controlMask1.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);
        Color[] mask2Color = controlMask2.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);
        Color[] paintAreaColor1 = PaintAreaTexture1.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);
        Color[] paintAreaColor2 = PaintAreaTexture2.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);

        //获取笔刷蒙像素版值
        float ratio = (float)brush.BrushMask.width / brush.Size;
        Color[] brusMaskColor = brush.BrushMask.GetPixels(
            (int)(brushOffsetX * ratio),
            (int)(brushOffestY * ratio),
            (int)(brushRangeWidth * ratio),
            (int)(brushRangeHeight * ratio),
            0);

        int index = 0;
        //设置像素颜色
        for (int w = 0; w < brushRangeWidth; w++)
        {
            for (int h = 0; h < brushRangeHeight; h++)
            {

                index = (int)(h * ratio) * (int)(brushRangeWidth * ratio) + (int)(w * ratio);
                index = index < brusMaskColor.Length ? index : brusMaskColor.Length - 1;

                intensity = PaintAreaCorrect(
                    brusMaskColor[index].r * brush.Intensity * inkRemain * 0.1f,
                    GetPaintAreaColor(brush.Channel, paintAreaColor1[h * brushRangeWidth + w], paintAreaColor2[h * brushRangeWidth + w]));

                PaintOnChannel(
                    brush.Channel,
                    intensity,
                    ref mask1Color[h * brushRangeWidth + w],
                    ref mask2Color[h * brushRangeWidth + w]);
            }
        }

        brush.UseInk(1);
        controlMask1.SetPixels(x, y, brushRangeWidth, brushRangeHeight, mask1Color, 0);
        controlMask2.SetPixels(x, y, brushRangeWidth, brushRangeHeight, mask2Color, 0);
        controlMask2.Apply();
        controlMask1.Apply();
    }

    float GetPaintAreaColor(int channel, Color paintAreaColor1, Color paintAreaColor2)
    {
        switch (channel)
        {
            case 0:
                return paintAreaColor1.r;
            case 1:
                return paintAreaColor1.g;
            case 2:
                return paintAreaColor1.b;
            case 3:
                return paintAreaColor1.a;
            case 4:
                return paintAreaColor2.r;
            case 5:
                return paintAreaColor2.g;
            case 6:
                return paintAreaColor2.b;
            case 7:
                return paintAreaColor2.a;
            default:
                return 0;
        }
    }
    float PaintAreaCorrect(float channelValue, float paintAreaValue)
    {
        return Smoothstep(Correction, 1, channelValue + paintAreaValue * Correction);
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
        mask2.a = Mathf.Lerp(mask2.a, channel == 7 ? 1 : 0, channelValue);
    }

    float Smoothstep(float t1, float t2, float x)
    {
        x = Mathf.Clamp01((x - t1) / (t2 - t1));
        return x;
    }

    public bool SaveRenderTextureToPNG(Texture2D texture, string pngName)
    {
        string path = Application.dataPath + "/Textures/" + pngName + ".png";
        byte[] _bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, _bytes);

        Debug.LogError("Save: " + path);
        return true;

    }
}


public enum PointType
{
    StartPoint = -1,
    MiddlePoint = 0,
    EndPoint = 1,
}