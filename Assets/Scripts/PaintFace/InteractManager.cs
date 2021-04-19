using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//Interact Manager
//1.绘制√
//2.道具选择
//3.相机操控
//4.界面切换
public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    
    //脸谱绘制
    Vector2 lasHitUv;
    void UpdatePaintMesh()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity))
            {
                if (hitInfo.collider.gameObject != FaceTextureManager.Instance.FaceRenderer.gameObject)
                {
                    return;
                }
                if (PaintLevelManager.Instance.CurrentBrush == null)
                {
                    Debug.LogError("当前笔刷为空");
                    return;
                }

                Vector2 hitUV = hitInfo.textureCoord2;
                if (Input.GetMouseButtonDown(0))
                {
                    FaceTextureManager.Instance.ClearFrameBuffer();
                    PaintLevelManager.Instance.CurrentBrush.ResetInk();
                    lasHitUv = hitUV;//起始点不适用上一次绘制的uv
                }

                FaceTextureManager.Instance.Paint(hitUV, lasHitUv, PaintLevelManager.Instance.CurrentBrush);
                lasHitUv = hitUV;

            }
        }
    }

    //道具选择
    PaintToolObj currentPaintToolObj;
    void UpdatePaintToolSelect()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity))
        {
            if (hitInfo.collider.gameObject.CompareTag("PaintToolObj"))
            {
                //悬浮
                if (!currentPaintToolObj)
                {
                    currentPaintToolObj = hitInfo.collider.gameObject.GetComponent<PaintToolObj>();
                    currentPaintToolObj.OnMouseEnter();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    currentPaintToolObj.OnMouseClick();
                }
            }
            else
            {
                if (currentPaintToolObj)
                {
                    currentPaintToolObj.OnMouseExit();
                    currentPaintToolObj = null;
                }
            }
        }
        else
        {
             if (currentPaintToolObj)
                {
                    currentPaintToolObj.OnMouseExit();
                    currentPaintToolObj = null;
                }
        }
    }
    


   

    void UpatePanel()
    {

    }
    void Update()
    {

        UpdatePaintMesh();
        UpdatePaintToolSelect();
        CameraController.Instance.UpdateCamera();

    }
}
