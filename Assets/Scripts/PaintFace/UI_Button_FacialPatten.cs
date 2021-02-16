using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_FacialPatten : MonoBehaviour
{
    private FacialPattenScriptableObject facialPatten;
    private UI_Panel_ChooseFacialPatten panel;
   
    public void SetFacialPatternBtn(FacialPattenScriptableObject _facialPatten)
    {
        facialPatten = _facialPatten;
        GetComponentInChildren<Image>().sprite = facialPatten.PreviewImage;
        GetComponentInChildren<Text>().text = facialPatten.FaceName;
        GetComponent<Button>().onClick.AddListener(OnClick);
        panel=GetComponentInParent<UI_Panel_ChooseFacialPatten>();
        gameObject.SetActive(true);
    }
    public void OnClick()
    {
        PaintLevelManager.Instance.SetCurrentFace(facialPatten);        
        panel.SetFacialPattenInfo(facialPatten);
    }
}
