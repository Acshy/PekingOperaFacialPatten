using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShake : MonoBehaviour
{
    public float Speed;
    public float Range;
    Light light;
    private void Start()
    {
        light = GetComponent<Light>();
    }
    // Update is called once per frame
    void Update()
    {
        light.intensity=1+Mathf.Sin(Time.time*Speed)*Range;
    }
}
