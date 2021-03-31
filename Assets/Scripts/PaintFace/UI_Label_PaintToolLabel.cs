using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Label_PaintToolLabel : MonoBehaviour
{
    public Text PaintToolNameText;
    public Text PaintToolInfoText;

    public void ShowLabel(string name, string info, Vector3 objPos)
    {
        PaintToolNameText.text = name;
        PaintToolInfoText.text = info;
        gameObject.transform.position = Camera.main.WorldToScreenPoint(objPos);
        gameObject.SetActive(true);
    }
    public void HideLabel()
    {
        gameObject.SetActive(false);
    }
}
