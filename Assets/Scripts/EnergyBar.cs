using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    // Código da aula de UI do professor Kleber.
    [Header("UI")]
    public Image m_Bar;
    public Text m_GaugeNumberText;

    [Header("Settings")]
    public float m_MaxValue;

    [Header("Time")]
    public bool m_UseTime;
    public float m_FillTime;

    [Header("Player")]
    public PlayerHealth m_PlayerHealth;

    private float m_Value;
    private float m_ElapsedTime;
    private int m_GaugeNumber;

    private void Start()
    {
        m_MaxValue = 50.0f;
        float energy = m_PlayerHealth.m_Energy;
        UpdateHealth(energy);
    }

    void Update()
    {
        m_GaugeNumberText.text = m_GaugeNumber.ToString();
    }

    public void SetIncrementalValue(float value, bool useTime)
    {
        if (useTime)
        {
            StartCoroutine(UpdateHealthTime(m_Value, Mathf.Clamp(m_Value + value, 0.0f, m_MaxValue)));
        }
        else
        {
            UpdateHealth(value);
        }
    }

    private void UpdateHealth(float value)
    {
        m_Value += value;
        if(m_Value >= m_MaxValue)
        {
            m_Value -= m_MaxValue;
            m_GaugeNumber++;
        }
        if(m_Value <= 0)
        {
            if(m_GaugeNumber > 0)
            {
                m_Value += m_MaxValue;
                m_GaugeNumber--;
            }
        }
        //m_Value = Mathf.Clamp(m_Value + value, 0.0f, m_MaxValue);

        m_Bar.fillAmount = m_Value / m_MaxValue;
    }

    private IEnumerator UpdateHealthTime(float fromValue, float toValue)
    {
        m_ElapsedTime = 0.0f;
        while (m_ElapsedTime / m_FillTime < 1.0f)
        {
            m_ElapsedTime += Time.deltaTime;
            m_Value = Mathf.Lerp(fromValue, toValue, m_ElapsedTime / m_FillTime);
            m_Bar.fillAmount = m_Value / m_MaxValue;
            yield return null;
        }
    }
}
