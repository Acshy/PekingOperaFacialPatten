using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_FacialPatten : MonoBehaviour
{
    private FacialPattenScriptableObject facialPatten;
    public UI_Panel_InfoPanel InfoPanel;
    public UI_Panel_FacialGroup FacialGroupPanel;
   
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
        InfoPanel.SetFacialPatten(facialPatten);
        InfoPanel.Show();
        FacialGroupPanel.Hide();
    }
}
