using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintAreaHintController : MonoBehaviour
{
    public float MaxValue;
    public float BlackCorrection=1;
    public float threshold = 0;
    public float lifeCycle=1;
    private SkinnedMeshRenderer faceRender;
    [SerializeField]private bool hitOn = false;
    void Start()
    {
        faceRender = FaceTextureManager.Instance.FaceRenderer;
    }

    public void SetHint(bool isHitOn)
    {
        hitOn = isHitOn;
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = 0;
        if (hitOn)
        {
           
            alpha = Mathf.Sin(Time.time * (6.28f / lifeCycle));
            alpha = Mathf.Lerp(0, 1, alpha - threshold) * MaxValue;
            if (PaintLevelManager.Instance.CurrentBrush.Channel == 0)
                alpha *= BlackCorrection;
        }
        faceRender.material.SetFloat("_EmissionScale",alpha);
    }
}
