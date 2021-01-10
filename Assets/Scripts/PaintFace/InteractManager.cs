using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [InlineEditor]
    public BrushScriptableObject TestBrush;
    Brush currentBrush;
    public Transform HeadCenter;
    Vector2 lasHitUv;

    Transform cameraTrans;
    Vector3 mouseDownPoint;
    Vector3 mouseDownCamFwd;
    float cameraDistance;
    public Vector2 CameraRotSpeed;
    void Start()
    {
        cameraTrans = Camera.main.transform;
        cameraDistance = Vector3.Distance(cameraTrans.position, HeadCenter.transform.position);
        currentBrush = new Brush(TestBrush);
    }

    private void OnValidate() {
         currentBrush = new Brush(TestBrush);
    }
    void Update()
    {

        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity))
            {
                Vector2 hitUV = hitInfo.textureCoord;
                PointType pointType = PointType.MiddlePoint;
                if (Input.GetMouseButtonDown(0))
                {
                    lasHitUv = hitUV;
                    pointType=PointType.StartPoint;
                }
                
                if(Input.GetMouseButtonUp(0))
                {
                     pointType=PointType.EndPoint;
                }

                if (hitInfo.collider.gameObject == FaceTextureManager.Instance.FaceRenderer.gameObject)
                {
                    //currentBrush = new Brush(TestBrush);
                    FaceTextureManager.Instance.Paint(hitUV, lasHitUv, currentBrush,pointType);
                    lasHitUv = hitUV;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            mouseDownPoint = Input.mousePosition;
            mouseDownCamFwd = cameraTrans.forward;
        }
        if (Input.GetMouseButton(1))
        {
            cameraTrans.forward =
                Quaternion.AngleAxis((Input.mousePosition - mouseDownPoint).x * CameraRotSpeed.x, cameraTrans.up) *
                mouseDownCamFwd;
            Vector3 pos = HeadCenter.position - cameraTrans.forward * cameraDistance;
            pos.y = cameraTrans.position.y;
            cameraTrans.position = pos;

        }
    }
}
