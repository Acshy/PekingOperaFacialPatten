using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    None = 0,
    Story = 1,
    Study = 2,
    Paint = 3,
    Create = 4
}
public class SceneManager : MonoBehaviour
{
    static public SceneManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {           
            LoadScene(0);
        }
    }



    public void LoadScene(int id)
    {
        UnloadCurrentScene();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);
    }
    void UnloadCurrentScene()
    {
        int id = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(id);
    }

}
