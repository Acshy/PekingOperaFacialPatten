using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterButtonManager : MonoBehaviour
{
    
    void Start()
    {

    }

    public void OnClickBtn(int gameModeID)
    {
        SceneManager.Instance.LoadScene(gameModeID);
        GameMode gameMode = (GameMode)gameModeID;
        switch (gameMode)
        {
            case GameMode.None:
                break;
            case GameMode.Story:
                break;
            case GameMode.Study:
                break;
            case GameMode.Paint:
                break;
            case GameMode.Create:
                break;
        }
    }
    
}
