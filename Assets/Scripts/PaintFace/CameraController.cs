using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
   
    public int DefaultCameraPosIndex=0;
    
    //相机旋转控制
    [ReadOnly][SerializeField]private Transform TargetTrans;
    public float ZoomSpeed;
    public float RotXSpeed;
    public float RotYSpeed;
    
    Vector3 mouseDownEuler;
    public float Lerp = 0.5f;
    Transform cameraTrans;
    Vector3 mouseDownPoint;
    Vector3 mouseDownCamFwd;
    float cameraDistance;
    [SerializeField]private bool freeControl = true;

    [Serializable]
    public struct CameraData
    {
        public Transform CameraPointTrans;
        public Transform LookAtPoint;
        public bool FreeController;
    }
    public List<CameraData> CameraDataList;
    void Start()
    {
        cameraTrans = Camera.main.transform;
        cameraDistance = Vector3.Distance(cameraTrans.position, TargetTrans.transform.position);
        ApplyCameraData(DefaultCameraPosIndex);
    }
    

    IEnumerator DisableCameraControl(float time, bool freeControlOnComplete)
    {
        freeControl = false;
        yield return new WaitForSeconds(time);
        freeControl = freeControlOnComplete;
    }

    public void ApplyCameraData(int cameraPointIndex ,float time=0)
    {
        CameraData cameraData = CameraDataList[cameraPointIndex];
        TargetTrans = cameraData.LookAtPoint;
        if(cameraData.LookAtPoint!=null)
        {
            cameraData.CameraPointTrans.LookAt(cameraData.LookAtPoint);   
        }
        
        if (time == 0)
        {
            cameraTrans.position = cameraData.CameraPointTrans.position;
            cameraTrans.rotation = cameraData.CameraPointTrans.rotation;
        }
        else
        {
            cameraTrans.DOMove(cameraData.CameraPointTrans.position, time).SetEase(Ease.InOutSine);
            cameraTrans.DORotateQuaternion(cameraData.CameraPointTrans.rotation, time).SetEase(Ease.InOutSine);
            StartCoroutine(DisableCameraControl(time,cameraData.FreeController));
        }
    }

    public void UpdateCamera()
    {
        if (freeControl)
        {
            //鼠标控制
            float wheel = Input.mouseScrollDelta.y * Time.deltaTime * ZoomSpeed;
            cameraTrans.transform.Translate(Vector3.forward * wheel);
            cameraDistance = Vector3.Distance(cameraTrans.position, TargetTrans.transform.position);
            Vector3 pos = TargetTrans.position - cameraTrans.forward * cameraDistance;
            if (Input.GetMouseButtonDown(1))
            {
                mouseDownPoint = Input.mousePosition;
                mouseDownCamFwd = cameraTrans.forward;
                mouseDownEuler = cameraTrans.eulerAngles;
            }
            if (Input.GetMouseButton(1))
            {
                Vector3 moseMove = (Input.mousePosition - mouseDownPoint);
                Vector3 targetEuler = mouseDownEuler + new Vector3(-moseMove.y * RotYSpeed, moseMove.x * RotXSpeed, 0);
                cameraTrans.rotation = Quaternion.Lerp(cameraTrans.rotation, Quaternion.Euler(targetEuler), Lerp);
                cameraTrans.position = TargetTrans.position - cameraTrans.forward * cameraDistance;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ApplyCameraData(0,0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ApplyCameraData(1,0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ApplyCameraData(2,0.5f );
        }
    }
}
