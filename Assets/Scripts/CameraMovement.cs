using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject m_p1, m_p2; // Associar os dois lutadores
    public float m_yCorrection, m_LimitDistance, m_MinLimit, m_MaxLimit;
    private float m_P1OldX, m_P2OldX;
    // private int testando = 2; 
    // public GameObject testandoFighter;
    /*
    m_yCorrection: Devido à função UpdateCamY zerar o valor de y, esta variavel serve para arrumar a posição da camera
    m_LimitDistance: Associar a distancia maxima entre dois players
    m_MinLimit, m_MaxLimit: Introduzir o valor correspondente ao menor ponto e o maior ponto em x da arena respectivamente.
    */

    void Update()
    {
        // Primeiro, a posição da camera é atualizada com base no posicionamento dos lutadores
        transform.position = MoveCamera(m_p1.transform.position, m_p2.transform.position);
        // Então, as validações conferem se algum dos lutadores está tentando ultrapassar os limites da arena
        if (m_p1.transform.position.x < m_MinLimit || m_p1.transform.position.x > m_MaxLimit)
            Limiter(m_p1);
        if (m_p2.transform.position.x < m_MinLimit || m_p2.transform.position.x > m_MaxLimit)
            Limiter(m_p2);
        // E por fim, verifica se os lutadores estão ultrapassando o limite de distancia entre os dois jogadores.
        if (Vector3.Distance(m_p1.transform.position, m_p2.transform.position) > m_LimitDistance)
        {
            Bounds(m_p1, m_p2, Mathf.Min(m_P1OldX, m_P2OldX) + m_LimitDistance);
        }
        else
        {
            m_P1OldX = m_p1.transform.position.x;
            m_P2OldX = m_p2.transform.position.x;
        }

        // //teste
        //     if(Input.GetButtonDown("Fire" + testando))
        //     {
        //         Debug.Log("entrou no fire");
        //         testandoFighter.transform.Rotate(testandoFighter.transform.rotation.x, 180, testandoFighter.transform.rotation.z);
        //     }
        // //fimteste

    }

    private Vector3 MoveCamera(Vector3 p1, Vector3 p2)
    {
        // Chama as funções para atualizar a posição da camera
        Vector3 res = new Vector3(UpdateCamX(p1, p2), (UpdateCamY(p1, p2) + m_yCorrection), transform.position.z);
        return res;
    }

    private float UpdateCamX(Vector3 p1, Vector3 p2)
    {
        // Calcula a distancia entre o x dois jogadores e posiciona a camera na metade
        Vector3 xPos = Vector3.Lerp(p1, p2, 0.5f);
        return xPos.x;
    }

    private float UpdateCamY(Vector3 p1, Vector3 p2)
    {
        // Calcula a distancia entre o y dois jogadores e posiciona a camera na metade
        Vector3 yPos = Vector3.Lerp(p1, p2, 0.5f);
        return yPos.y;
    }

    private void Limiter(GameObject player)
    {
        // Mantém os players dentro dos limites definidos.
        player.transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, m_MinLimit, m_MaxLimit), player.transform.position.y, player.transform.position.z);
    }

    private void Bounds(GameObject player1, GameObject player2, float stopPoint)
    {
        player1.transform.position = new Vector3(Mathf.Clamp(player1.transform.position.x, stopPoint - m_LimitDistance, stopPoint), player1.transform.position.y, player1.transform.position.z);
        player2.transform.position = new Vector3(Mathf.Clamp(player2.transform.position.x, stopPoint - m_LimitDistance, stopPoint), player2.transform.position.y, player2.transform.position.z);
    }
}
