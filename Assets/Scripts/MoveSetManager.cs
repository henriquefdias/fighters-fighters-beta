using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSetManager : MonoBehaviour
{
    public float m_ElapsedTime = 0.0f;
    public float m_ComboWaitTime = 0.2f;
    public string m_Combo;
    private MoveDict m_MoveDict;
    private Movement m_Movement;
    private string m_PlayerNumber;
    public string[] m_SpecialMove;
    private float m_CurrentPowerGauge;
    private bool m_FoundAttack;
    private bool m_IsCrouched;
    private bool m_IsStopped;
    private string[] m_AttackInputList;
    private string[] m_AttackMoveList;
    private string m_CurrentAttackMove;
    private Animator m_Anim;

    private PlayerHealth m_PHealth;

    private void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_PHealth = GetComponent<PlayerHealth>();
        m_MoveDict = GetComponent<MoveDict>();
        m_Movement = GetComponent<Movement>();
        m_PlayerNumber = m_Movement.m_IsP1 ? "P1" : "P2";
        m_AttackInputList = new string[] { "Soco1" + m_PlayerNumber, "Soco2" + m_PlayerNumber, "Chute1" + m_PlayerNumber, "Chute2" + m_PlayerNumber, "Defesa" + m_PlayerNumber };
        m_AttackMoveList = new string[] { "S1", "S2", "C1", "C2", "D" };
    }

    void Update()
    {
        m_CurrentPowerGauge = m_PHealth.m_Energy;
        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("AndandoParaTras") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("AndandoParaFrente"))
        {
            if (m_Movement.m_IsP1 || (!m_Movement.m_IsP1 && !PreFightManager.m_IsVsCPU))
                CheckInputs();
        }
    }

    private void CheckInputs()
    {
        if (!m_Movement.m_IsDefending)
        {
            if (!FightMainSceneMan.m_IsPaused)
            {
                m_ElapsedTime += Time.deltaTime;

                if (m_ElapsedTime > m_ComboWaitTime && m_Combo.Length != 0)
                {
                    m_Combo = "";
                    m_ElapsedTime = 0.0f;
                }

                if (Input.GetAxisRaw("Horizontal" + m_PlayerNumber) > 0)
                {
                    if (m_IsStopped)
                    {
                        m_Anim.SetBool("Idle", false);
                        m_Anim.SetBool("Back", false);
                        m_Anim.SetBool("Front", true);
                        m_IsStopped = false;
                        m_Combo += m_Movement.m_AtLeft ? "F" : "T";
                        m_ElapsedTime = 0.0f;
                    }
                }
                else if (Input.GetAxisRaw("Horizontal" + m_PlayerNumber) < 0)
                {
                    if (m_IsStopped)
                    {
                        m_Anim.SetBool("Idle", false);
                        m_Anim.SetBool("Front", false);
                        m_Anim.SetBool("Back", true);
                        m_IsStopped = false;
                        m_Combo += m_Movement.m_AtLeft ? "T" : "F";
                        m_ElapsedTime = 0.0f;
                    }
                }
                else
                {
                    m_IsStopped = true;
                    m_Anim.SetBool("Front", false);
                    m_Anim.SetBool("Back", false);
                    m_Anim.SetBool("Idle", true);
                }


                //if (Input.GetButtonDown("Vertical" + m_PlayerNumber))
                //{
                //    Debug.Log("105 FM GOOOOOOOOOOOOOOL");
                //    if (Input.GetAxisRaw("Vertical" + m_PlayerNumber) < 0)
                //    {
                //        m_Combo += "B";
                //        m_ElapsedTime = 0.0f;
                //    }
                //}

                if (Input.GetAxisRaw("Vertical" + m_PlayerNumber) < 0)
                {
                    if (!m_IsCrouched)
                    {
                        m_IsCrouched = true;
                        m_Combo += "B";
                        m_ElapsedTime = 0.0f;
                    }
                }
                else
                    m_IsCrouched = false;


                if (Input.GetButtonDown(m_AttackInputList[0]))
                {
                    m_CurrentAttackMove = m_AttackMoveList[0];
                    m_FoundAttack = true;
                }

                if (Input.GetButtonDown(m_AttackInputList[1]))
                {
                    m_CurrentAttackMove = m_AttackMoveList[1];
                    m_FoundAttack = true;
                }

                if (Input.GetButtonDown(m_AttackInputList[2]))
                {
                    m_CurrentAttackMove = m_AttackMoveList[2];
                    m_FoundAttack = true;
                }

                if (Input.GetButtonDown(m_AttackInputList[3]))
                {
                    m_CurrentAttackMove = m_AttackMoveList[3];
                    m_FoundAttack = true;
                }

                if (Input.GetButtonDown(m_AttackInputList[4]))
                {
                    m_CurrentAttackMove = m_AttackMoveList[4];
                    m_FoundAttack = true;
                }

                if (m_FoundAttack)
                {
                    Debug.Log("entrou no found attack");
                    m_FoundAttack = false;
                    m_Combo += m_CurrentAttackMove;
                    int comboLength = m_Combo.Length;

                    for (int i = 0; i < comboLength; i++)
                    {
                        Debug.Log(m_Combo);
                        m_SpecialMove = m_MoveDict.SearchMove(m_Combo);
                        if (m_SpecialMove != null)
                        {
                            int power = int.Parse(m_SpecialMove[2]);
                            if (power <= m_CurrentPowerGauge)
                            {
                                if (power > 0)
                                    //m_PHealth.IncreaseEnergy(power * -1, true);

                                    Debug.Log(m_SpecialMove[0]); // TODO: Aqui entra o "SetTrigger"
                                m_Anim.SetTrigger(m_SpecialMove[0]);
                                break;
                            }
                            else
                            {
                                Debug.Log("Insufficient power");
                                m_Combo = m_Combo.Substring(1);
                                continue;
                            }
                        }
                        else
                        {
                            if (m_Combo.Length <= 1)
                            {
                                Debug.Log("apenas um no combo");
                                break;
                            }
                            else
                            {
                                m_Combo = m_Combo.Substring(1);
                            }
                        }
                    }
                    m_ElapsedTime = 0.0f;
                    m_Combo = "";
                }


                /*
                        if (Input.GetKeyDown(KeyCode.J)) // TODO: tentar fazer um ifzao com todos os inputs de golpes
                        {
                            m_FoundAttack = false;
                            m_Combo += "S1";
                            int comboLength = m_Combo.Length;

                            for (int i = 0; i < comboLength; i++)
                            {
                                Debug.Log(m_Combo);
                                m_SpecialMove = m_MoveDict.SearchMove(m_Combo);
                                if (m_SpecialMove != null)
                                {
                                    int power = int.Parse(m_SpecialMove[2]);
                                    if (power <= m_CurrentPowerGauge)
                                    {
                                        Debug.Log(m_SpecialMove[0].ToString()); // TODO: Aqui entra o "SetTrigger"
                                        break;
                                    }
                                    else
                                    {
                                        Debug.Log("Insufficient power");
                                        m_Combo = m_Combo.Substring(1);
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (m_Combo.Length <= 1)
                                    {
                                        Debug.Log("apenas um no combo");
                                        break;
                                    }
                                    else
                                    {
                                        m_Combo = m_Combo.Substring(1);
                                    }
                                }
                            }

                            m_ElapsedTime = 0.0f;
                            m_Combo = "";
                        }
                */
            }
        }
    }
}
