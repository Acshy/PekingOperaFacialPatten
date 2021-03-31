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




    void Start()
    {
        cameraTrans = Camera.main.transform;
        cameraDistance = Vector3.Distance(cameraTrans.position, HeadCenter.transform.position);
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
    


    //相机旋转控制
    public Transform HeadCenter;
    [HorizontalGroup("Zoom", LabelWidth = 50)] public float ZoomSpeed;
    [HorizontalGroup("Zoom")] public float MinZ;
    [HorizontalGroup("Zoom")] public float MaxZ;

    [HorizontalGroup("RotX", LabelWidth = 50)] public float RotXSpeed;
    [HorizontalGroup("RotX")] public float MinX;
    [HorizontalGroup("RotX")] public float MaxX;

    [HorizontalGroup("RotY", LabelWidth = 50)] public float RotYSpeed;
    [HorizontalGroup("RotY")] public float MinY;
    [HorizontalGroup("RotY")] public float MaxY;
    Vector3 mouseDownEuler;
    public float Lerp = 0.5f;
    Transform cameraTrans;
    Vector3 mouseDownPoint;
    Vector3 mouseDownCamFwd;
    float cameraDistance;
    void UpdateCamera()
    {
        float wheel = Input.mouseScrollDelta.y * Time.deltaTime * 10;
        cameraTrans.transform.Translate(Vector3.forward * wheel);
        cameraDistance = Vector3.Distance(cameraTrans.position, HeadCenter.transform.position);
        Vector3 pos = HeadCenter.position - cameraTrans.forward * cameraDistance;
        if (Input.GetMouseButtonDown(1))
        {
            mouseDownPoint = Input.mousePosition;
            mouseDownCamFwd = cameraTrans.forward;
            mouseDownEuler = cameraTrans.eulerAngles;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 moseMove = (Input.mousePosition - mouseDownPoint);
            Vector3 targetEuler = mouseDownEuler + new Vector3(-moseMove.y * RotXSpeed, moseMove.x * RotYSpeed, 0);
            cameraTrans.rotation = Quaternion.Lerp(cameraTrans.rotation, Quaternion.Euler(targetEuler), Lerp);
            cameraTrans.position = HeadCenter.position - cameraTrans.forward * cameraDistance;
        }

        //限制检测
    }


    void UpatePanel()
    {

    }
    void Update()
    {

        UpdatePaintMesh();
        UpdatePaintToolSelect();
        UpdateCamera();

    }
}
