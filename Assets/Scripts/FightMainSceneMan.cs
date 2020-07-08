using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightMainSceneMan : MonoBehaviour
{
    public Vector3 m_P1Pos;
    public Vector3 m_P2Pos;
    public GameObject m_P1GO;
    public GameObject m_P2GO;
    public Movement m_P1Mov;
    public Movement m_P2Mov;
    public PlayerHealth m_P1Health;
    public PlayerHealth m_P2Health;
    public Transform[] m_P1Components;
    public Transform[] m_P2Components;
    public MoveSetManager m_P1MoveSet;
    public MoveSetManager m_P2MoveSet;

    public GameObject m_PauseScreen;
    public GameObject m_ResumeGameButtonGO;
    public static bool m_IsPaused;
    public EventSystem m_ES;
    public StandaloneInputModule m_SIM;
    public Text m_PauseText;
    public string m_LastPlayerPaused;
    private bool m_IsFightHappening = true;

    [Header("Associate")]
    public HealthBar m_P1HealthBar;
    public HealthBar m_P2HealthBar;
    public CameraMovement m_Camera;
    public Timer m_VictoryTrigger;
    public Text m_VictoryText;
    public GameObject m_EndFightScreen;
    public GameObject m_FightAgainButton;
    public Text m_EndFightWinnerText;
    public SpriteRenderer m_ArenaBackground;
    public Renderer m_ArenaGround;

    void Awake()
    {
        Time.timeScale = 1;
        m_P1Pos = new Vector3(-5, 4.1f, 3);
        m_P2Pos = new Vector3(5, 4.1f, 3);
        m_IsPaused = m_PauseScreen.activeSelf;
        Instantiator();
        PlayerVariablesAssociator();
        ValueAssociator();
        SkinAssociator();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("StartP1"))
        {
            PauseFight("P1");
        }

        if (Input.GetButtonDown("StartP2"))
        {
            if (!PreFightManager.m_IsVsCPU)
            {
                PauseFight("P2");
            }
        }

        if (m_IsFightHappening)
        {
            if (m_P1Health.m_Health <= 0)
            {
                if (PreFightManager.m_IsVsCPU)
                {
                    SetVictoryAnim("CPU");
                    StartCoroutine(WaitForSomeSeconds(1.0f, "CPU", "P1"));
                    // TODO: ativar animação de vitoria da cpu
                }
                else
                {
                    SetVictoryAnim("Player 2");
                    StartCoroutine(WaitForSomeSeconds(1.0f, "P2", "P1"));
                    // TODO: ativar animação de vitoria do player 2                
                }

                m_IsFightHappening = false;
                Time.timeScale = 0.2f;
            }

            if (m_P2Health.m_Health <= 0)
            {
                SetVictoryAnim("Player 1");
                m_IsFightHappening = false;
                Time.timeScale = 0.2f;
                StartCoroutine(WaitForSomeSeconds(1.0f, "P1", "P2"));
                // TODO: ativar animação de vitoria do player 1
            }
        }
    }

    public void SetVictoryAnim(string winner)
    {
        //StartCoroutine("FightEnd");

        m_VictoryText.text = winner + " WINS!";
        m_VictoryTrigger.m_Anim.SetTrigger("Ativar");
    }

    IEnumerator FightEnd()
    {
        float elapsedTime = 0;

        while (elapsedTime < 5.0f)
        {
            elapsedTime += Time.deltaTime;

        }


        yield return new WaitForEndOfFrame();
    }

    public void PauseFight(string pNumber)
    {
        if (!m_IsFightHappening) return;

        if (m_LastPlayerPaused == "")
            m_LastPlayerPaused = pNumber;

        if (pNumber == "resume")
            pNumber = m_LastPlayerPaused;

        if (m_LastPlayerPaused == pNumber)
        {
            if (!m_IsPaused)
            {
                Time.timeScale = 0;
                m_IsPaused = true;
                m_PauseScreen.SetActive(true);
                m_ES.SetSelectedGameObject(m_PauseScreen);
                m_ES.SetSelectedGameObject(m_ResumeGameButtonGO);
                m_SIM.horizontalAxis = "Horizontal" + pNumber;
                m_SIM.verticalAxis = "Vertical" + pNumber;
                m_SIM.submitButton = "Chute1" + pNumber;
                m_PauseText.text = pNumber + " Pause";
            }
            else
            {
                Time.timeScale = 1;
                m_IsPaused = false;
                m_PauseScreen.SetActive(false);
                m_LastPlayerPaused = "";
            }
        }
    }

    public void QuitToMenuHandler()
    {
        Time.timeScale = 1;
        SceneMan.StaticLoadScene("MainMenuTest");
    }

    public void SceneLoader(string scene)
    {
        Time.timeScale = 1;
        SceneMan.StaticLoadScene(scene);
    }

    public void Instantiator()
    {
        int p1 = int.Parse(PreFightManager.m_P1FighterArray[0]);
        int p2 = int.Parse(PreFightManager.m_P2FighterArray[0]);
        // pega o index que esta na primeira posicao do vetor que monta o fighter.
        m_P1GO = Instantiate(PreFightManager.m_FightersPrefabs[p1], m_P1Pos, Quaternion.identity);
        m_P2GO = Instantiate(PreFightManager.m_FightersPrefabs[p2], m_P2Pos, Quaternion.identity);
    }

    public void PlayerVariablesAssociator()
    {
        m_P1Mov = m_P1GO.GetComponent<Movement>();
        m_P1Health = m_P1GO.GetComponent<PlayerHealth>();
        m_P1MoveSet = m_P1GO.GetComponent<MoveSetManager>();

        m_P2Mov = m_P2GO.GetComponent<Movement>();
        m_P2Health = m_P2GO.GetComponent<PlayerHealth>();
        m_P2MoveSet = m_P2GO.GetComponent<MoveSetManager>();
    }

    public void ValueAssociator()
    {
        m_P1Mov.m_Adversary = m_P2GO.transform;
        m_P1Mov.m_IsP1 = bool.Parse(PreFightManager.m_P1FighterArray[3]);
        m_P1Mov.m_AtLeft = true;
        m_P1Health.m_Bar = m_P1HealthBar;
        m_P1HealthBar.m_PlayerHealth = m_P1Health;
        m_P1Health.m_EnemyMove = m_P2MoveSet;


        m_P2Mov.m_Adversary = m_P1GO.transform;
        m_P2Mov.m_IsP1 = bool.Parse(PreFightManager.m_P2FighterArray[3]);
        m_P1Mov.m_AtLeft = false;
        m_P2Health.m_Bar = m_P2HealthBar;
        m_P2HealthBar.m_PlayerHealth = m_P2Health;
        m_P2Health.m_EnemyMove = m_P1MoveSet;

        m_Camera.m_p1 = m_P1GO;
        m_Camera.m_p2 = m_P2GO;

        m_ArenaBackground.sprite = PreFightManager.m_ArenaBackgrounds[PreFightManager.m_ArenaIndex];
        m_ArenaGround.material = PreFightManager.m_ArenaGrounds[PreFightManager.m_ArenaIndex];
    }

    public void ChangeTextColorToRed(Text text)
    {
        text.color = Color.red;
    }

    public void ChangeTextColorToWhite(Text text)
    {
        text.color = Color.white;
    }

    public void SetArenaBackground(int index)
    {
        m_ArenaBackground.sprite = PreFightManager.m_ArenaBackgrounds[index];
    }

    public void SetArenaGround(int index)
    {
        m_ArenaGround.material = PreFightManager.m_ArenaGrounds[index];
    }

    public void SkinAssociator()
    {
        int skinIndexP1 = int.Parse(PreFightManager.m_P1FighterArray[1]);
        int skinIndexP2 = int.Parse(PreFightManager.m_P2FighterArray[1]);

        m_P1Components = m_P1GO.GetComponentsInChildren<Transform>();
        m_P2Components = m_P2GO.GetComponentsInChildren<Transform>();

        foreach (Transform child in m_P1Components)
        {
            if (child.CompareTag("Skin"))
            {
                Renderer childRenderer = child.GetComponent<Renderer>();
                childRenderer.material = PreFightManager.m_SkinArray[skinIndexP1];
            }
        }

        foreach (Transform child in m_P2Components)
        {
            if (child.CompareTag("Skin"))
            {
                Renderer childRenderer = child.GetComponent<Renderer>();
                childRenderer.material = PreFightManager.m_SkinArray[skinIndexP2];
            }
        }

    }

    IEnumerator WaitForSomeSeconds(float seconds, string winner, string loser)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("BRABO COROUTINE FUNCIONANDO BALA");
        Time.timeScale = 0;
        m_EndFightScreen.SetActive(true);
        m_ES.SetSelectedGameObject(m_EndFightScreen);
        m_ES.SetSelectedGameObject(m_FightAgainButton);
        if (PreFightManager.m_IsVsCPU)
        {
            if (winner.Equals("P1"))
            {
                m_SIM.horizontalAxis = "Horizontal" + winner;
                m_SIM.verticalAxis = "Vertical" + winner;
                m_SIM.submitButton = "Chute1" + winner;
                m_EndFightWinnerText.text = winner + " Wins!";
            }
            else
            {
                m_SIM.horizontalAxis = "Horizontal" + loser;
                m_SIM.verticalAxis = "Vertical" + loser;
                m_SIM.submitButton = "Chute1" + loser;
                m_EndFightWinnerText.text = winner + " WINS!";
                Debug.Log("Chegou no change buttons");
            }
        }
        else
        {
            m_SIM.horizontalAxis = "Horizontal" + loser;
            m_SIM.verticalAxis = "Vertical" + loser;
            m_SIM.submitButton = "Chute1" + loser;
            m_EndFightWinnerText.text = winner + " WINS!";
            Debug.Log("Chegou no change buttons");
        }
    }
}
