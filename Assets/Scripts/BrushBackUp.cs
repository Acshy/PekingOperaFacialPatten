// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Brush : MonoBehaviour
// {
//     public Texture2D BrushMask;
//     public int Channel;
//     public int Size;    
//     public float Intensity;
//     public float MaxDistance;
//     public Brush(Texture2D _texture, int _size, int _channel, float _intensity, float _maxDistance)
//     {
//         BrushMask = _texture;
//         Size = _size;
//         Channel = _channel;
//         Intensity = _intensity;
//         MaxDistance = _maxDistance;
//     }

//     // public void Paint(Texture2D makeupTexture, Vector2 pointUV)
//     // {
//     //     if (makeupTexture == null)
//     //         return;


//     //     int x, y;//笔刷贴图的起始点位置
//     //     int clampX, calmpY;//限制在贴图内的XY
//     //     int brushRangeWidth, brushRangeHeight;//笔刷的处理后尺寸

//     //     //计算xy
//     //     pointUV = new Vector2(Mathf.Clamp01(pointUV.x), Mathf.Clamp01(pointUV.y));//归一化UV
//     //     x = Mathf.FloorToInt(pointUV.x * (float)makeupTexture.width - ((float)Size / 2));
//     //     y = Mathf.FloorToInt(pointUV.y * (float)makeupTexture.height - ((float)Size / 2));

//     //     //计算笔刷真实覆盖范围（主要处理边界的情况）
//     //     brushRangeWidth = Mathf.Min(Size, (makeupTexture.width - x), x + Size);
//     //     brushRangeHeight = Mathf.Min(Size, (makeupTexture.height - y), y + Size);

//     //     //把xy给规范到贴图范围内
//     //     clampX = Mathf.Clamp(x, 0, makeupTexture.width - Size / 2 - 1);
//     //     calmpY = Mathf.Clamp(y, 0, makeupTexture.height - Size / 2 - 1);
//     //     Color[] sourceColor = makeupTexture.GetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, 0);

//     //     Color[] resultColor = SetColorWithBrush(sourceColor, x, y, brushRangeWidth, brushRangeHeight);
//     //     makeupTexture.SetPixels(clampX, calmpY, brushRangeWidth, brushRangeHeight, resultColor, 0);
//     //     makeupTexture.Apply();

//     // }

//     // Color[] SetColorWithBrush(Color[] source, int x, int y, int width, int height)
//     // {
//     //     Color[] result = new Color[width * height];

//     //     int offsetX=0, offsetY=0;
//     //     if (width < Size)
//     //     {
//     //         offsetX = x < 0 ? Size - width : 0;
//     //         offsetY = y < 0 ? Size - height : 0;
//     //     }

//     //     for (int w = 0; w < width; w++)
//     //     {
//     //         for (int h = 0; h < height; h++)
//     //         {
//     //             //获取brush蒙板的像素值(还有优化空间)
//     //             float mask = BrushMask.GetPixel((w + offsetX) * BrushMask.width / Size, (h + offsetY) * BrushMask.height / Size).r;
//     //             mask *= Intensity;

//     //             //蒙板后的颜色
//     //             Color maskColor = BrushColor * Intensity;

//     //             //混合
//     //             result[h * width + w] = Color.Lerp(maskColor, source[h * width + w], 1 - mask);



//     //         }
//     //     }
//     //     return result;
//     // }
// }
