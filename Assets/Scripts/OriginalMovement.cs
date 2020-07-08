


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OriginalMovement : MonoBehaviour
{
    public float m_Speed = 10.0f;
    public float m_Jump = 20.0f;
    private Rigidbody m_Player;
    private Collider m_Col;
    public Transform m_Adversary;
    private float m_Mirroring;
    private bool m_AtLeft;
    public float maxJumpHeight = 3.0f;
    public Vector3 groundPos;
    public float jumpSpeed = 7.0f;
    public float fallSpeed = 12.0f;
    public bool inputJump = false;

    private void Awake()
    {
        m_Player = GetComponent<Rigidbody>();
        m_Col = GetComponent<Collider>();
    }

    private void Start()
    {
        m_AtLeft = transform.position.x > m_Adversary.position.x ? false : true;
        groundPos = transform.position;
    }

    private void Update()
    {
        TurnPlayer();
        float x = Input.GetAxis("Horizontal") * m_Speed * Time.deltaTime;
        Vector3 posit = new Vector3(x * m_Mirroring, 0.0f, 0.0f);
        transform.Translate(posit);


        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Entrou pulo");
            inputJump = true;
            StartCoroutine("Jump");
        }

        m_AtLeft = transform.position.x > m_Adversary.position.x ? false : true;

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, m_Col.bounds.extents.y + 0.2f);
    }

    private void TurnPlayer()
    {
        if (m_AtLeft)
        {
            m_Mirroring = -1.0f;
            transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
        }
        else
        {
            m_Mirroring = 1.0f;
            transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
        }
    }

    IEnumerator Jump()
    {
        while (true)
        {
            if (transform.position.y >= maxJumpHeight)
                inputJump = false;
            if (inputJump)
                transform.Translate(Vector3.up * jumpSpeed * Time.smoothDeltaTime);
            else if (!inputJump)
            {
                transform.Translate(Vector3.down * fallSpeed * Time.smoothDeltaTime);
                if (transform.position.y < groundPos.y)
                {
                    transform.position = groundPos;
                    StopAllCoroutines();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}