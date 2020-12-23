// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PainMeshTest : MonoBehaviour
// {
//     [Header("Test Brush")]
//     public Texture2D BrushTexture;
//     public int BrushSize;
//     public Color BrushColor;
//     public float BlushIntensity;
//     public float BrushMaxDistance;
//     Brush testBrush;

//     [Header("Texture")]
//     Texture2D makeUpTexture;


//     [Header("插值补间")]
//     float maxHitPointStep = 0.005f;
//     Vector2 lastHitPointUV;


//     void Start()
//     {
//         testBrush = new Brush(BrushTexture, BrushSize, BrushColor, BlushIntensity, BrushMaxDistance);
//         InitTexture();

//     }

//     void InitTexture()
//     {
//         Texture2D MeshMainTexture = (Texture2D)GetComponent<Renderer>().material.mainTexture;
//         makeUpTexture = new Texture2D(MeshMainTexture.width, MeshMainTexture.height, TextureFormat.ARGB32, true);
//         for (int x = 0; x < MeshMainTexture.width; x++)
//         {
//             for (int y = 0; y < MeshMainTexture.height; y++)
//             {
//                 makeUpTexture.SetPixel(x, y, MeshMainTexture.GetPixel(x, y));
//             }
//         }

//         GetComponent<Renderer>().material.SetTexture("_DrawTexture", makeUpTexture);
//     }

//     //在2个点之间创建补间
//     void InterpolatPoints(Vector2 pointA, Vector2 pointB)
//     {
//         //2个点距离差过大，一般是跨UV接缝或者其他问题导致的不连续，不进行补间
//         if(Vector2.SqrMagnitude(pointA-pointB)>0.5)
//         {
//             return;            
//         }
//         int insertPointCount = Mathf.RoundToInt(Vector2.Distance(pointA, pointB) / maxHitPointStep);
//         if (insertPointCount > 0)
//         {
//             for (int i = 0; i < insertPointCount; i++)
//             {
//                 Vector2 insertPointUV = pointB + (pointA - pointB).normalized * maxHitPointStep * i;
//                 insertPointUV = new Vector2(insertPointUV.x%1,insertPointUV.y%1);
//                 testBrush.Paint(makeUpTexture, insertPointUV);
//             }
//         }
//     }

//     void Update()
//     {

//         if (Input.GetMouseButton(0))
//         {
//             RaycastHit hitInfo = new RaycastHit();
//             if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity))
//             {
//                 Vector2 pointUV = hitInfo.textureCoord;
//                 testBrush.Paint(makeUpTexture, pointUV);

//                 //若是按下的第一帧，则初始化lastHitPointUV
//                 if (Input.GetMouseButtonDown(0))
//                 {
//                     lastHitPointUV = pointUV;
//                 }
//                 //若不是，则检测和上一帧的位置，进行插值
//                 else
//                 {
//                     InterpolatPoints(pointUV, lastHitPointUV);                    
//                     lastHitPointUV = pointUV;
//                 }
//             }

//         }
//     }
// }
