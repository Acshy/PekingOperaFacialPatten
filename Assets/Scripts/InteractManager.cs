using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public Brush TestBrush;

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity))
            {
                Vector2 hitUV = hitInfo.textureCoord;
                bool isStart = Input.GetMouseButtonDown(0);
                //TODO:调用绘制程序
                //if(hitInfo.collider.gameObject==FaceTextureManager.Instance.FaceRenderer.gameObject)
                {
                    FaceTextureManager.Instance.PaintColor(hitUV, TestBrush);
                }
            }
        }
    }
}
