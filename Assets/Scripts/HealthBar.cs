using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Código da aula de UI do professor Kleber.
    [Header("UI")]
    public Image m_Bar;

    [Header("Settings")]
    public float m_MaxValue;

    [Header("Time")]
    public bool m_UseTime;
    public float m_FillTime;

    [Header("Player")]
    public PlayerHealth m_PlayerHealth;

    private float m_Value;
    private float m_ElapsedTime;

    private void Start()
    {
        float health = m_PlayerHealth.m_Health;
        m_MaxValue = health;
        UpdateHealth(health);
    }

    public void SetIncrementalValue(float value, bool useTime)
    {
        //if (useTime)
        //{
        //    StartCoroutine(UpdateHealthTime(m_Value, Mathf.Clamp(m_Value + value, 0.0f, m_MaxValue)));
        //}
        //else
        //{
        //    UpdateHealth(value);
        //}

        m_Value += value;
        //m_Value = Mathf.Clamp(m_Value + value, 0.0f, m_MaxValue);

        m_Bar.fillAmount = m_Value / m_MaxValue;
    }

    private void UpdateHealth(float value)
    {
        m_Value = Mathf.Clamp(m_Value + value, 0.0f, m_MaxValue);
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
