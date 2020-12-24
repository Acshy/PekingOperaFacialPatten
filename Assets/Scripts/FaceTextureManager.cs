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
    public Texture2D baseMap;
    Texture2D resultBaseMap;
    //public Texture2D controlMask1;
    //public Texture2D controlMask2;

    float[,][] controlMask;

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
        resultBaseMap = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true);
        controlMask = new float[baseMap.width, baseMap.height][];
        //controlMask1 = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true);
        //controlMask2 = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true);
        for (int x = 0; x < baseMap.width; x++)
        {
            for (int y = 0; y < baseMap.height; y++)
            {
                resultBaseMap.SetPixel(x, y, baseMap.GetPixel(x, y));
                controlMask[x, y] = new float[PaletteManager.Instance.ChannelColors.Length];
                //controlMask1.SetPixel(x, y, new Color(0, 0, 0, 0));
                //controlMask2.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        resultBaseMap.Apply();
        //controlMask1.Apply();
        //controlMask2.Apply();
        renderer.material.SetTexture("_BaseMap", resultBaseMap);
    }

    ProfilerMarker preparePerfMarker = new ProfilerMarker("MyTEST");
    public void PaintColor(Vector2 pointUV, Brush brush)
    {
        if (baseMap == null)
            return;


        int x, y;//笔刷贴图的起始点位置
        int clampX, calmpY;//限制在贴图内的XY
        int brushRangeWidth, brushRangeHeight;//笔刷的处理后尺寸
        int brushOffsetX, brushOffestY;

        //计算xy
        pointUV = new Vector2(Mathf.Clamp01(pointUV.x), Mathf.Clamp01(pointUV.y));//归一化UV
        x = Mathf.FloorToInt(pointUV.x * (float)resultBaseMap.width - ((float)brush.Size / 2));
        y = Mathf.FloorToInt(pointUV.y * (float)resultBaseMap.height - ((float)brush.Size / 2));

        //计算笔刷真实覆盖范围（主要处理边界的情况）
        brushRangeWidth = Mathf.Min(brush.Size, (resultBaseMap.width - x), x + brush.Size);
        brushRangeHeight = Mathf.Min(brush.Size, (resultBaseMap.height - y), y + brush.Size);

        //把xy给规范到贴图范围内
        clampX = Mathf.Clamp(x, 0, resultBaseMap.width - brush.Size / 2 - 1);
        calmpY = Mathf.Clamp(y, 0, resultBaseMap.height - brush.Size / 2 - 1);

        //笔刷起点偏移
        brushOffsetX = x < 0 ? brush.Size - brushRangeWidth : 0;
        brushOffestY = y < 0 ? brush.Size - brushRangeHeight : 0;

        //获取贴图像素颜色
        //Color[] mask1Color = controlMask1.GetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, 0);
        //Color[] mask2Color = controlMask2.GetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, 0);
        Color[] resultColor = baseMap.GetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, 0);

        preparePerfMarker.Begin();


        Color paintColor;
        float maskSum = 0;
        float mask = 0;

        Color[] maskColor = brush.BrushMask.GetPixels(
            brushOffsetX * brush.BrushMask.width / brush.Size,
            brushOffestY * brush.BrushMask.height / brush.Size,
            brushRangeWidth * brush.BrushMask.width / brush.Size,
            brushRangeHeight * brush.BrushMask.height / brush.Size,
            0);

        //设置像素颜色
        for (int w = 0; w < brushRangeWidth; w++)
        {
            for (int h = 0; h < brushRangeHeight; h++)
            {
                //mask = brush.Intensity * brush.BrushMask.GetPixel((w + brushOffsetX) * brush.BrushMask.width / brush.Size, (h + brushOffestY) * brush.BrushMask.height / brush.Size).r;
                mask = 
                    maskColor[h * brush.BrushMask.height / brush.Size*
                    brushRangeWidth * brush.BrushMask.width / brush.Size + 
                    w *brush.BrushMask.width / brush.Size].r;
                PaletteManager.Instance.PaintOnChannel(
                   brush.Channel,
                   mask*brush.Intensity*Time.deltaTime,
                   ref controlMask[clampX + w, calmpY + h]);

                paintColor = PaletteManager.Instance.GetMixedColor(controlMask[clampX + w, calmpY + h]);
                maskSum = 0;
                for (int i = 0; i < controlMask[clampX + w, calmpY + h].Length; i++)
                {
                    maskSum += controlMask[clampX + w, calmpY + h][i];
                }
                // PaletteManager.Instance.PaintOnChannel(
                //     brush.Channel, mask,
                //     ref mask1Color[h * brushRangeWidth + w],
                //     ref mask2Color[h * brushRangeWidth + w],
                //     out paintColor,
                //     out maskSum);

                resultColor[h * brushRangeWidth + w] = resultColor[h * brushRangeWidth + w] * (1 - maskSum) + paintColor * maskSum;
            }
        }
        preparePerfMarker.End();

        //controlMask1.SetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, mask1Color, 0);
        //controlMask1.Apply();
        //controlMask2.SetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, mask2Color, 0);
        //controlMask2.Apply();
        resultBaseMap.SetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, resultColor, 0);
        resultBaseMap.Apply();

    }
    // Color[] SetColorInRect(Color[] source, Brush brush, int)
    // {
    //     Color[] result = new Color[width * height];

    //     int offsetX = 0, offsetY = 0;
    //     if (width < brush.Size)
    //     {
    //         offsetX = x < 0 ? brush.Size - width : 0;
    //         offsetY = y < 0 ? brush.Size - height : 0;
    //     }

    //     for (int w = 0; w < width; w++)
    //     {
    //         for (int h = 0; h < height; h++)
    //         {
    //             //获取brush蒙板的像素值(还有优化空间)
    //             float mask = brush.BrushMask.GetPixel((w + offsetX) * brush.BrushMask.width / brush.Size, (h + offsetY) * brush.BrushMask.height / brush.Size).r;
    //             float value = mask * brush.Intensity;



    //             Color mask1 = controlMask1.GetPixel();

    //             PaletteManager.Instance.PaintOnChannel(brush.Channel, value);

    //             //混合
    //             float lerp = 0;
    //             for (int i = 0; i < colorMask[w, h].Length; i++)
    //             {
    //                 lerp += colorMask[w, h][i];
    //             }
    //             result[h * width + w] = Color.Lerp(source[h * width + w], Color.red, lerp);
    //             colorMask[w, h][i] += value;
    //             result[h * width + w] = Color.Lerp(source[h * width + w], Color.red, value);
    //         }
    //     }

    //     return result;
    // }
}
