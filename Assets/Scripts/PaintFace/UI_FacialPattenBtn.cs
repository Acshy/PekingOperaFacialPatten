using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FacialPattenBtn : MonoBehaviour
{
    private FacialPatten facialPatten;
    private UI_ChooseFacialPattenPanel panel;
    public void SetFacialPatternBtn(FacialPatten _facialPatten)
    {
        facialPatten = _facialPatten;
        GetComponentInChildren<Image>().sprite = facialPatten.Image;
        GetComponentInChildren<Text>().text = facialPatten.FaceName;
        GetComponent<Button>().onClick.AddListener(OnClick);
        gameObject.SetActive(true);
    }
    public void OnClick()
    {
        PaintLevelManager.Instance.SetCurrentFace(facialPatten);
    }
}
