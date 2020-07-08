using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {

    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void StaticLoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitScene()
    {
        Application.Quit();
    }
}
