using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;


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
    public Texture2D paintAreaMask1;
    public Texture2D paintAreaMask2;
    Texture2D controlMask1;
    Texture2D controlMask2;

    public float AdjustValue=0.25f;
    public float InterpolateDis = 0.1f;
    //float[,][] controlMask;

    private void Start()
    {
        Initialized(FaceRenderer);
    }

    private void Initialized(Renderer renderer)
    {
        //获取颜色贴图
        if (renderer)
            baseMap = (Texture2D)renderer.sharedMaterial.GetTexture("_BaseMap");


        //初始化结果图
        //resultBaseMap = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true);
        //controlMask = new float[baseMap.width, baseMap.height][];
        controlMask1 = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true);
        controlMask2 = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true);
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

    public void PaintColor(Vector2 currentUV, Vector2 lastUV, Brush brush)
    {
        if (lastUV == Vector2.zero)
            return;
        if (baseMap == null)
            return;

        Vector2 vec2 = lastUV - currentUV;
        Vector2 dir = vec2.normalized;

        int pointCount = (int)Mathf.Sqrt(Vector2.SqrMagnitude(new Vector2(vec2.x * baseMap.width, vec2.y * baseMap.height)) / brush.Size / 2);
        float stepDis = Vector2.Distance(lastUV, currentUV) / pointCount;

        if (pointCount <= 1 || Vector2.SqrMagnitude(vec2) > 0.25f)
        {
            PaintColor(currentUV, brush);
        }
        else
        {
            for (int i = 0; i < pointCount; i++)
            {
                PaintColor(currentUV + dir * stepDis * i, brush);
            }
        }
    }
    public void PaintColor(Vector2 pointUV, Brush brush)
    {
        if (baseMap == null)
            return;

        int x, y;//笔刷贴图在模型uv上的起始点位置
        int clampX, calmpY;//限制在贴图内的XY
        int brushRangeWidth, brushRangeHeight;//笔刷的处理后尺寸
        int brushOffsetX, brushOffestY;

        //计算xy
        pointUV = new Vector2(Mathf.Clamp01(pointUV.x), Mathf.Clamp01(pointUV.y));//归一化UV
        x = Mathf.FloorToInt(pointUV.x * (float)baseMap.width - ((float)brush.Size / 2));
        y = Mathf.FloorToInt(pointUV.y * (float)baseMap.height - ((float)brush.Size / 2));

        //计算笔刷真实覆盖范围（主要处理边界的情况）
        brushRangeWidth = Mathf.Min(brush.Size, (baseMap.width - x), x + brush.Size);
        brushRangeHeight = Mathf.Min(brush.Size, (baseMap.height - y), y + brush.Size);

        //把xy给规范到贴图范围内
        clampX = Mathf.Clamp(x, 0, baseMap.width - brush.Size / 2 - 1);
        calmpY = Mathf.Clamp(y, 0, baseMap.height - brush.Size / 2 - 1);

        //笔刷起点偏移
        brushOffsetX = x < 0 ? brush.Size - brushRangeWidth : 0;
        brushOffestY = y < 0 ? brush.Size - brushRangeHeight : 0;

        //获取贴图像素颜色
        Color[] mask1Color = controlMask1.GetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, 0);
        Color[] mask2Color = controlMask2.GetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, 0);
        Color[] paintAreaMask1Color = paintAreaMask1.GetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, 0);
        Color[] paintAreaMask2Color = paintAreaMask2.GetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, 0);


        float mask = 0;
        int index = 0;
        float ratio = (float)brush.BrushMask.width / brush.Size;
        //获取笔刷蒙版值
        Color[] brusMaskColor = brush.BrushMask.GetPixels(
            (int)(brushOffsetX * ratio),
            (int)(brushOffestY * ratio),
            (int)(brushRangeWidth * ratio),
            (int)(brushRangeHeight * ratio),
            0);

        //设置像素颜色
        for (int w = 0; w < brushRangeWidth; w++)
        {
            for (int h = 0; h < brushRangeHeight; h++)
            {

                index = (int)(h * ratio) * (int)(brushRangeWidth * ratio) + (int)(w * ratio);
                index = index < brusMaskColor.Length ? index : brusMaskColor.Length - 1;
                mask = brusMaskColor[index].r;

                PaintOnChannel(
                    brush.Channel,
                    mask * brush.Intensity * Time.deltaTime,
                    paintAreaMask1Color[h * brushRangeWidth + w],
                    paintAreaMask2Color[h * brushRangeWidth + w],
                    ref mask1Color[h * brushRangeWidth + w],
                    ref mask2Color[h * brushRangeWidth + w]);

            }
        }

        controlMask1.SetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, mask1Color, 0);
        controlMask2.SetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, mask2Color, 0);
        controlMask2.Apply();
        controlMask1.Apply();
    }

    public void PaintOnChannel(int channel, float channelValue, Color paintArea1, Color paintArea2, ref Color mask1, ref Color mask2)
    {
        mask1.r = Mathf.Lerp(mask1.r, channel == 0 ? 1 : 0, channelValue);
        mask1.g = Mathf.Lerp(mask1.g, channel == 1 ? 1 : 0, channelValue);
        mask1.b = Mathf.Lerp(mask1.b, channel == 2 ? 1 : 0, channelValue);
        mask1.a = Mathf.Lerp(mask1.a, channel == 3 ? 1 : 0, channelValue);
        mask2.r = Mathf.Lerp(mask2.r, channel == 4 ? 1 : 0, channelValue);
        mask2.g = Mathf.Lerp(mask2.g, channel == 5 ? 1 : 0, channelValue);
        mask2.b = Mathf.Lerp(mask2.b, channel == 6 ? 1 : 0, channelValue);
        mask2.a = Mathf.Lerp(mask2.b, channel == 6 ? 1 : 0, channelValue);
    }
}
