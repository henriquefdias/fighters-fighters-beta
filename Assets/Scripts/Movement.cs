using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float m_Speed = 10.0f;
    private Rigidbody m_Player;
    private Collider m_Col;
    public Transform m_Adversary;
    public float m_Mirroring;

    public float m_MaxJumpHeight;
    public float m_OldGroundPosY;
    public float m_JumpSpeed;
    public float m_FallSpeed;

    public bool m_IsP1 = false;
    public bool m_AtLeft;
    public bool m_OldAtLeft;
    public bool m_InputJump = false;
    public bool m_IsDefending = false;
    public bool m_IsJumping = false;
    private string m_PlayerNumber;
    private Animator m_Anim;


    private void Awake()
    {

    }

    private void Start()
    {
        m_Player = GetComponent<Rigidbody>();
        m_Col = GetComponent<Collider>();
        m_Anim = GetComponent<Animator>();
        m_OldAtLeft = transform.position.x > m_Adversary.position.x ? false : true;
        m_PlayerNumber = m_IsP1 ? "P1" : "P2";
        m_OldGroundPosY = transform.position.y;
        StartingRotation();
        Mirror();
    }

    private void Update()
    {
        m_AtLeft = transform.position.x > m_Adversary.position.x ? false : true;

        if (m_AtLeft != m_OldAtLeft)
        {
            Mirror();
            TurnPlayer();
        }

        if (m_IsP1 || (!m_IsP1 && !PreFightManager.m_IsVsCPU))
        {
            float x = Input.GetAxisRaw("Horizontal" + m_PlayerNumber) * m_Speed * Time.smoothDeltaTime * m_Mirroring;
            transform.Translate(x, 0.0f, 0.0f); // Gira o personagem de acordo com que lado da tela ele está

            if (Input.GetButton("Defesa" + m_PlayerNumber))
            {
                m_IsDefending = true;
                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Front", false);
                m_Anim.SetBool("Back", false);
                m_Anim.SetBool("Jump", false);
                m_Anim.SetBool("Crouch", false);
                m_Anim.SetBool("Defense", true);
            }
            else
            {
                m_IsDefending = false;
                m_Anim.SetBool("Defense", false);
            }

            Debug.Log(Input.GetAxisRaw("Vertical" + m_PlayerNumber));
            Debug.Log(IsGrounded());
            if (!m_IsJumping)
            {
                if (Input.GetAxisRaw("Vertical" + m_PlayerNumber) > 0)
                {
                    m_Anim.SetBool("Jump", true);
                    m_IsJumping = true;
                    m_InputJump = true;
                    StartCoroutine("Jump");
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    SceneManager.LoadScene("FightScene");
        //}
    }

    public bool IsGrounded() // TODO: MELHORAR ESSA DETECÇÃO, POIS PERMITE UM SEGUNDO PULO QUANDO ESTÁ EM CIMA DO ADVERSÁRIO
    {
        return Physics.Raycast(transform.position, Vector3.down, m_Col.bounds.extents.y + 0.0002f);
    }

    public void Mirror()
    {
        m_Mirroring = m_AtLeft ? 1.0f : -1.0f;
        m_OldAtLeft = m_AtLeft;
    }
    private void TurnPlayer()
    {
        transform.Rotate(transform.rotation.x, 180, transform.rotation.z);
    }

    private void StartingRotation()
    {
        transform.Rotate(0, 180, 0);
    }

    IEnumerator Jump()
    {
        while (true)
        {
            if (transform.position.y >= m_MaxJumpHeight)
                m_InputJump = false;
            if (m_InputJump)
                transform.Translate(Vector3.up * m_JumpSpeed * Time.smoothDeltaTime);
            else if (!m_InputJump)
            {
                transform.Translate(Vector3.down * m_FallSpeed * Time.smoothDeltaTime);
                if (transform.position.y < m_OldGroundPosY)
                {
                    transform.position = new Vector3(transform.position.x, m_OldGroundPosY, transform.position.z);
                    m_IsJumping = false;
                    m_Anim.SetBool("Jump", false);
                    StopAllCoroutines();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}