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
    Vector2 lasHitUv;
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
                if (Input.GetMouseButtonDown(0))
                    lasHitUv = Vector2.zero;


                if (hitInfo.collider.gameObject == FaceTextureManager.Instance.FaceRenderer.gameObject)
                {
                    FaceTextureManager.Instance.PaintColor(hitUV, lasHitUv, TestBrush);
                    lasHitUv = hitUV;
                }
            }
        }
    }
}
