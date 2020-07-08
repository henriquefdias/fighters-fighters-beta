using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text m_Timer;
    public float m_TimeLeft = 99.0f;
    public Animator m_Anim;

    void Start()
    {
        m_Timer = GetComponent<Text>();
        InvokeRepeating("TimerCounter", 0, 1);
    }

    private void TimerCounter()
    {
        if (m_TimeLeft > 0)
        {
            m_TimeLeft --;
            m_Timer.text = m_TimeLeft.ToString("0");
        }
        else
        {
            m_Timer.text = "0";
            m_Anim.SetTrigger("Ativar");
        }
    }
}
