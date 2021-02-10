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
    Texture2D colorMask1;
    Texture2D colorMask2;
    Texture2D specularMask;

    public float Correction;
    public Texture2D PaintAreaTexture1;
    public Texture2D PaintAreaTexture2;

    Color[] lastFrameMask1Color;
    Color[] lastFrameMask2Color;

    private void Start()
    {
        Initialized(FaceRenderer);
        PaintActionQueue = new List<IEnumerator>();
    }

    private void Initialized(Renderer renderer)
    {
        //获取颜色贴图
        if (!renderer)
        {
            return;
        }

        baseMap = new Texture2D(512, 512);//(Texture2D)renderer.sharedMaterial.GetTexture("_BaseMap");
        colorMask1 = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true, true);
        colorMask2 = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true, true);
        specularMask = new Texture2D(baseMap.width, baseMap.height, TextureFormat.ARGB32, true, true);

        for (int x = 0; x < baseMap.width; x++)
        {
            for (int y = 0; y < baseMap.height; y++)
            {
                colorMask1.SetPixel(x, y, new Color(0, 0, 0, 0));
                colorMask2.SetPixel(x, y, new Color(0, 0, 0, 0));
                specularMask.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        colorMask1.Apply();
        colorMask2.Apply();
        specularMask.Apply();

        renderer.material.SetTexture("_ControlMask1", colorMask1);
        renderer.material.SetTexture("_ControlMask2", colorMask2);
        renderer.material.SetTexture("_SpecularMask2", specularMask);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveRenderTextureToPNG(colorMask1, "TextureSaved_Mask1");
        }
        //每一帧从绘制队列中取值绘制
        if (PaintActionQueue.Count > 0)
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
            if (brush.BrushType == BrushType.Smudge)//为涂抹模式清空帧缓存
            {
                lastFrameMask1Color = null;
                lastFrameMask2Color = null;
            }
        }

        Vector2 vec2 = lastUV - currentUV;
        int pointCount = 1;
        if (Vector2.SqrMagnitude(vec2) < 0.25f)
        {
            pointCount = (int)(Vector2.Distance(new Vector2(vec2.x * baseMap.width, vec2.y * baseMap.height), Vector2.zero) / brush.Size * brush.Continuity);
            pointCount = pointCount < 1 ? 1 : pointCount;
        }
        float stepDis = Vector2.Distance(lastUV, currentUV) / pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            PaintActionQueue.Add(DelayPaint(lastUV - vec2.normalized * stepDis * i, brush));
        }


    }

    List<IEnumerator> PaintActionQueue;
    IEnumerator DelayPaint(Vector2 pointUV, Brush brush)
    {
        Paint(pointUV, brush);
        yield return null;
    }

    public void Paint(Vector2 pointUV, Brush brush)
    {
        if (baseMap == null)
            return;
        float correction;
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


        //获取笔刷蒙像素版值
        float ratio = (float)brush.BrushMask.width / brush.Size;
        Color[] brusMaskColor = brush.BrushMask.GetPixels(
            (int)(brushOffsetX * ratio),
            (int)(brushOffestY * ratio),
            (int)(brushRangeWidth * ratio),
            (int)(brushRangeHeight * ratio),
            0);


        //若是颜色笔刷
        if (brush.BrushType == BrushType.Color)
        {
            //获取控制模型贴图像素值
            Color[] colorMask1Color = colorMask1.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);
            Color[] colorMask2Color = colorMask2.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);
            Color[] specularMaskColor = specularMask.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);

            //获取绘制区域贴图像素值
            Color[] paintAreaColor;
            if (brush.Channel < 4)
            {
                paintAreaColor = PaintAreaTexture1.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);
            }
            else
            {
                paintAreaColor = PaintAreaTexture2.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);
            }
            int index_brush = 0;
            //设置像素颜色
            for (int w = 0; w < brushRangeWidth; w++)
            {
                for (int h = 0; h < brushRangeHeight; h++)
                {

                    index_brush = (int)(h * ratio) * (int)(brushRangeWidth * ratio) + (int)(w * ratio);
                    index_brush = Mathf.Min(index_brush, brusMaskColor.Length - 1);

                    correction = CorrectPainArea(
                        brusMaskColor[index_brush].r * inkRemain,
                        GetPaintAreaColor(brush.Channel, paintAreaColor[h * brushRangeWidth + w]));

                    PaintColorOnChannel(
                        brush.Channel,
                        brusMaskColor[index_brush].r * brush.Intensity * correction,
                        ref colorMask1Color[h * brushRangeWidth + w],
                        ref colorMask2Color[h * brushRangeWidth + w]);
                    PaintSmoothness(
                        brush.SurfaceSmoothness,
                        brusMaskColor[index_brush].r * brush.Intensity * correction,
                        ref specularMaskColor[h * brushRangeWidth + w]);
                }
            }

            colorMask1.SetPixels(x, y, brushRangeWidth, brushRangeHeight, colorMask1Color, 0);
            colorMask2.SetPixels(x, y, brushRangeWidth, brushRangeHeight, colorMask2Color, 0);
            specularMask.SetPixels(x, y, brushRangeWidth, brushRangeHeight, specularMaskColor, 0);
            colorMask2.Apply();
            colorMask1.Apply();
            specularMask.Apply();
        }
        else if (brush.BrushType == BrushType.Smudge)
        {
            //获取控制模型贴图像素值
            Color[] mask1Color = colorMask1.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);
            Color[] mask2Color = colorMask2.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);

            if (lastFrameMask1Color != null && lastFrameMask2Color != null)
            {
                int index_brush = 0;
                int index_lastFrame = 0;
                //设置像素颜色
                for (int w = 0; w < brushRangeWidth; w++)
                {
                    for (int h = 0; h < brushRangeHeight; h++)
                    {

                        index_brush = (int)(h * ratio) * (int)(brushRangeWidth * ratio) + (int)(w * ratio);
                        index_brush = Mathf.Min(index_brush, brusMaskColor.Length - 1);
                        index_lastFrame = Mathf.Min(h * brushRangeWidth + w, lastFrameMask1Color.Length - 1);
                        float intensity = brusMaskColor[index_brush].r * brush.Intensity * inkRemain;
                        PaintSmudge(
                            intensity,
                            lastFrameMask1Color[index_lastFrame],
                            lastFrameMask2Color[index_lastFrame],
                            ref mask1Color[h * brushRangeWidth + w],
                            ref mask2Color[h * brushRangeWidth + w]);
                    }
                }
            }

            lastFrameMask1Color = mask1Color;
            lastFrameMask2Color = mask2Color;

            colorMask1.SetPixels(x, y, brushRangeWidth, brushRangeHeight, mask1Color, 0);
            colorMask2.SetPixels(x, y, brushRangeWidth, brushRangeHeight, mask2Color, 0);
            colorMask2.Apply();
            colorMask1.Apply();
        }
        else if (brush.BrushType == BrushType.Smooth)
        {
            //获取控制模型贴图像素值
            Color[] specularMaskColor = specularMask.GetPixels(x, y, brushRangeWidth, brushRangeHeight, 0);


            int index_brush = 0;
            //设置像素颜色
            for (int w = 0; w < brushRangeWidth; w++)
            {
                for (int h = 0; h < brushRangeHeight; h++)
                {

                    index_brush = (int)(h * ratio) * (int)(brushRangeWidth * ratio) + (int)(w * ratio);
                    index_brush = Mathf.Min(index_brush, brusMaskColor.Length - 1);

                    correction = brusMaskColor[index_brush].r * brush.Intensity * inkRemain;

                    PaintSmoothness(
                        brush.SurfaceSmoothness,
                        brusMaskColor[index_brush].r * brush.Intensity ,
                         ref specularMaskColor[h * brushRangeWidth + w]);
                }
            }

            specularMask.SetPixels(x, y, brushRangeWidth, brushRangeHeight, specularMaskColor, 0);
            specularMask.Apply();
        }



        brush.UseInk();
    }

    float GetPaintAreaColor(int channel, Color paintAreaColor)
    {
        switch (channel < 4 ? channel : channel - 4)
        {
            case 0:
                return paintAreaColor.r;
            case 1:
                return paintAreaColor.g;
            case 2:
                return paintAreaColor.b;
            case 3:
                return paintAreaColor.a;
            default:
                return 0;
        }
    }


    public void PaintColorOnChannel(int channel, float channelValue, ref Color mask1, ref Color mask2)
    {
        if (channel < 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == channel)
                    mask1[i] = Mathf.Clamp01(mask1[i] + channelValue);
                else
                    mask1[i] = Mathf.Clamp01(mask1[i] - channelValue);

                mask2[i] = Mathf.Clamp01(mask2[i] - channelValue);
            }
        }
        else
        {
            channel -= 4;
            for (int i = 0; i < 4; i++)
            {
                if (i == channel)
                    mask2[i] = Mathf.Clamp01(mask2[i] + channelValue);
                else
                    mask2[i] = Mathf.Clamp01(mask2[i] - channelValue);

                mask1[i] = Mathf.Clamp01(mask1[i] - channelValue);
            }
        }
    }
    float Smoothstep(float t1, float t2, float x)
    {
        x = Mathf.Clamp01((x - t1) / (t2 - t1));
        return x;
    }
    float CorrectPainArea(float channelValue, float paintAreaValue)
    {

        return Smoothstep(Correction, 1, channelValue + paintAreaValue * Correction);
    }
    public void PaintSmoothness(float Smoothness, float intensity, ref Color mask)
    {
        mask = Color.Lerp(mask,new Color(Smoothness,Smoothness,Smoothness,1),intensity);
    }
    public void PaintSmudge(float intensity, Color lastFrame1, Color lastFrame2, ref Color mask1, ref Color mask2)
    {
        mask1.r = Mathf.Lerp(mask1.r, lastFrame1.r, intensity);
        mask1.g = Mathf.Lerp(mask1.g, lastFrame1.g, intensity);
        mask1.b = Mathf.Lerp(mask1.b, lastFrame1.b, intensity);
        mask1.a = Mathf.Lerp(mask1.a, lastFrame1.a, intensity);
        mask2.r = Mathf.Lerp(mask2.r, lastFrame2.r, intensity);
        mask2.g = Mathf.Lerp(mask2.g, lastFrame2.g, intensity);
        mask2.b = Mathf.Lerp(mask2.b, lastFrame2.b, intensity);
        mask2.a = Mathf.Lerp(mask2.a, lastFrame2.a, intensity);
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