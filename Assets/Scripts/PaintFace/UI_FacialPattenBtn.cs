using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FacialPattenBtn : MonoBehaviour
{
    private FacialPattenScriptableObject facialPatten;
    private UI_ChooseFacialPattenPanel panel;
    public void SetFacialPatternBtn(FacialPattenScriptableObject _facialPatten)
    {
        facialPatten = _facialPatten;
        GetComponentInChildren<Image>().sprite = facialPatten.PreviewImage;
        GetComponentInChildren<Text>().text = facialPatten.FaceName;
        GetComponent<Button>().onClick.AddListener(OnClick);
        gameObject.SetActive(true);
    }
    public void OnClick()
    {
        PaintLevelManager.Instance.SetCurrentFace(facialPatten);
    }
}
