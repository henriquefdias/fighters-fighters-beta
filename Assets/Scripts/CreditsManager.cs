using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    public TextAsset m_TextFile;
    public Text m_TextUI;
    public Credits m_Credits;

    public void LoadMainMenu()
    {
        SceneMan.StaticLoadScene("MainMenuTest");
    }

    private void Start() 
    {
        string json = m_TextFile.text;
        m_Credits = JsonUtility.FromJson<Credits>(json);
        StringBuilder sb = new StringBuilder();

        foreach (Function function in m_Credits.functions)
        {
            sb.Append($"<b><size=50>{function.title}</size></b>");
            sb.Append($"<size=50>\n</size>");
            foreach (string name in function.people)
            {
                sb.Append($"<b><size=30>{name}</size></b>");
                sb.Append($"<size=30>\n</size>");
            }
            sb.Append($"<size=50>\n</size>");
        }

        m_TextUI.text = sb.ToString();
        Canvas.ForceUpdateCanvases();
    }

    public void Update()
    {
        m_TextUI.transform.Translate(Vector3.up * 60.0f * Time.deltaTime);
    }
}

[Serializable]
public class Credits
{
    public Function[] functions;
}

[Serializable]
public class Function 
 {
    public string title;
    public string[] people;
}