using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Código com a referencia basica para teste da barra de saúde
    public float m_Health;
    public float m_Energy;
    public float m_MaxEnergy;
    public HealthBar m_Bar; // Associar o GameObject barra de vida aqui
    private Movement m_Movement;
    public float m_CurrentEnemyDamage;
    private Animator m_Anim;
    //public EnergyBar m_ThisEnergyBar;

    //public EnergyBar m_EnemyEnergyBar;
    public PlayerHealth m_EnemyHealth;
    public MoveSetManager m_EnemyMove;

    public float m_hitValue = -10.0f; //Esta variavel recebera o valor do dano recebido

    void Awake()
    {
        m_Movement = GetComponent<Movement>();
        m_Anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_Health = 300.0f;
        m_Energy = 0;
    }

    //public void IncreaseEnergy(float value, bool toMe)
    //{
    //    if (toMe)
    //    {
    //        m_Energy += value;
    //        m_ThisEnergyBar.SetIncrementalValue(value, false);
    //    }
    //    else
    //    {

    //        m_Energy += value;
    //        m_EnemyEnergyBar.SetIncrementalValue(value, false);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log(this.gameObject.name);
            m_CurrentEnemyDamage = float.Parse(m_EnemyMove.m_SpecialMove[1]);
            if (m_Movement.m_IsDefending)
            {
                transform.Translate(m_CurrentEnemyDamage * -0.03f, 0, 0);
                m_Bar.SetIncrementalValue(m_CurrentEnemyDamage * -0.5f, true);
                m_Health -= m_CurrentEnemyDamage * 0.5f;
                m_Energy += m_CurrentEnemyDamage * 0.15f;
                //m_ThisEnergyBar.SetIncrementalValue(m_CurrentEnemyDamage * 0.15f, false);
                //if (float.Parse(m_EnemyMove.m_SpecialMove[2]) == 0) // se nao for super golpe, ele adiciona energia
                //{
                    //m_EnemyHealth.m_Energy += m_CurrentEnemyDamage * 0.25f;
                    //m_EnemyEnergyBar.SetIncrementalValue(m_CurrentEnemyDamage * 0.25f, false);
                //}
            }
            else
            {
                m_Anim.SetTrigger("DamageTaken");
                transform.Translate(m_CurrentEnemyDamage * -0.05f, 0, 0);
                m_Bar.SetIncrementalValue(m_CurrentEnemyDamage * -1, true);
                m_Health -= m_CurrentEnemyDamage;
                m_Energy += m_CurrentEnemyDamage * 0.2f;
                //m_ThisEnergyBar.SetIncrementalValue(m_CurrentEnemyDamage * 0.2f, false);
                //if (float.Parse(m_EnemyMove.m_SpecialMove[2]) == 0) // se nao for super golpe, ele adiciona energia
                //{
                    //m_EnemyHealth.m_Energy += m_CurrentEnemyDamage * 0.5f;
                    //m_EnemyEnergyBar.SetIncrementalValue(m_CurrentEnemyDamage * 0.5f, false);
                //}
            }
        }
    }
}
