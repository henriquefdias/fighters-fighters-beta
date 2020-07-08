using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyScript : MonoBehaviour
{
	private GameObject m_Audio;
	
	private void Awake()
	{
        m_Audio = GameObject.Find("BackgroundMusic");
		if(m_Audio == null)
		{
			m_Audio = this.gameObject;
			m_Audio.name = ("BackgroundMusic");
			DontDestroyOnLoad(m_Audio);
		}
		else
		{
			if(this.gameObject.name != "BackgroundMusic")
			{
				Destroy(this.gameObject);
			}
		}
	}

    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Credits") || SceneManager.GetActiveScene().name.Equals("FightScene"))
            Destroy(this.gameObject);
    }
}
