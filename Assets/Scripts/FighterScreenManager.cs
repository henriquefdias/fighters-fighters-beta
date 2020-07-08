using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FighterScreenManager : MonoBehaviour
{
    public GameObject[] m_FightersPrefab;
    private GameObject m_FigSelP1; //carrega o botao do fighter selecionado pelo player
    private GameObject m_FigSelP2; //carrega o botao do fighter selecionado pelo player
    public GameObject m_FighterSelectGO;
    public GameObject m_SkinSelectGO;
    public GameObject m_ArenaSelectGO;
    public EventSystem m_ES;
    public StandaloneInputModule m_SIM;
    public Text m_ChoosingTest;

    void Start()
    {
        PreFightManager.m_IsP1Choosing = true;
        ChangingControls("P1");
        m_FighterSelectGO.SetActive(true);
        FeedIsPlayer();
        ChangeTextChoosing(true);
    }

    public void ChangingControls(string pNumber)
    {
        m_SIM.horizontalAxis = "Horizontal" + pNumber;
        m_SIM.verticalAxis = "Vertical" + pNumber;
        m_SIM.submitButton = "Chute1" + pNumber;
    }

    public void FeedFighter(int index)
    {
        if (PreFightManager.m_IsP1Choosing)
        {
            PreFightManager.m_P1FighterArray[0] = index.ToString(); // aqui vai o index correspondente ao prefab no array de prefabs
        }
        else
        {
            PreFightManager.m_P2FighterArray[0] = index.ToString();
        }
        // feed array com index do lutador
    }
    public void FeedSkin(int index)
    {
        if (PreFightManager.m_IsP1Choosing)
            PreFightManager.m_P1FighterArray[1] = index.ToString();// aqui vai o index correspondente ao prefab no array de prefabs
        else
            PreFightManager.m_P2FighterArray[1] = index.ToString();
        // feed array com index da skin
    }

    public void FeedIsPlayer()
    {
        // alimenta o array com a informação de se é player ou não
        if (PreFightManager.m_IsP1Choosing)
            PreFightManager.m_P1FighterArray[2] = bool.TrueString;
        else
        {
            if (PreFightManager.m_IsVsCPU)
                PreFightManager.m_P2FighterArray[2] = bool.FalseString;
            else
                PreFightManager.m_P2FighterArray[2] = bool.TrueString;
        }
    }

    public void AfterChoseSkin()
    {
        // o que fará depois que o P1 escolhe a skin, alterando pra escolha do P2
        if (PreFightManager.m_IsP1Choosing)
        {
            PreFightManager.m_IsP1Choosing = false;
            if(!PreFightManager.m_IsVsCPU)
            {
                ChangingControls("P2");
            }
            m_FighterSelectGO.SetActive(true);
            m_ES.SetSelectedGameObject(m_FigSelP1);
            FeedIsPlayer();
            ChangeTextChoosing(false);
        }
        else
        {
            m_ChoosingTest.text = "";
        }
    }

    public void SetArenaSelectGOActive(GameObject selArena)
    {
        // coloca como ativa a tela de seleção de arena, mas apenas quando terminar de escolher o p2
        if (!PreFightManager.m_IsP1Choosing)
        {
            m_ArenaSelectGO.SetActive(true);
            m_ES.SetSelectedGameObject(selArena);
        }
    }

    public void ChooseArena(int index)
    {
        // escolhe a arena, alimenta o static arena index pra saber qual plano de fundo carregar,
        // carrega cena da luta, e esta irá instanciar os players e mudar o plano de fundo de acordo com os valores nas variaveis statics de "PreFightManager".
        Debug.Log("Chegou aqui! Selecionou arena!");
        PreFightManager.m_ArenaIndex = index;
        // TODO: Carregar cena da luta
        for (int i = 0; i < PreFightManager.m_P1FighterArray.Length; i++)
        {
            Debug.Log("P1 fighter: " + PreFightManager.m_P1FighterArray[i]);
        }
        for (int i = 0; i < PreFightManager.m_P2FighterArray.Length; i++)
        {
            Debug.Log("P2 fighter: " + PreFightManager.m_P2FighterArray[i]);
        }
        Debug.Log("Arena: " + PreFightManager.m_ArenaIndex);
    }

    public void SetEventSystem()
    {

    }

    public void SetSelectedFighter(GameObject selFighter)
    {
        // simplesmente guarda o botao que foi selecionado por ultimo
        if (PreFightManager.m_IsP1Choosing)
        {
            m_FigSelP1 = selFighter;
        }
        else
        {
            m_FigSelP2 = selFighter;
        }
    }

    public void ChangeTextChoosing(bool toP1)
    {
        // muda o texto no topo da tela, que informa qual fighter esta sendo escolhido
        if (toP1)
        {
            m_ChoosingTest.text = "CHOOSING P1";
        }
        else
        {
            if (PreFightManager.m_IsVsCPU)
            {
                m_ChoosingTest.text = "CHOOSING CPU";
            }
            else
            {
                m_ChoosingTest.text = "CHOOSING P2";
            }
        }
    }

    public void LoadScene(string scene)
    {
        SceneMan.StaticLoadScene(scene);
    }

    void Update()
    {
        // detecta se o player apertar o botão de voltar
        if (Input.GetButtonDown("DefesaP1"))
        {
            if (PreFightManager.m_IsP1Choosing)
            {
                if (PreFightManager.m_P1FighterArray[0] != "")
                {
                    // ele ja esta selecionando a skin, ou seja: trocar pra escolha de fighter
                    PreFightManager.m_P1FighterArray[0] = "";
                    m_SkinSelectGO.SetActive(false);
                    m_ES.SetSelectedGameObject(m_FigSelP1);
                }
                else
                {
                    // TODO: aparecer tela de "voltar ao menu principal?"
                    // SceneManagement.LoadScene("MainMenu")
                }
            }
            else
            {
                if (PreFightManager.m_IsVsCPU)
                {
                    if (PreFightManager.m_P2FighterArray[0] != "")
                    {
                        // ele ja esta selecionando a skin, ou seja: trocar pra escolha de fighter
                        PreFightManager.m_P2FighterArray[0] = "";
                        m_SkinSelectGO.SetActive(false);
                        m_ES.SetSelectedGameObject(m_FigSelP2);
                    }
                }
            }
        }
        if (Input.GetButtonDown("DefesaP2"))
        {
            if (!PreFightManager.m_IsP1Choosing)
            {
                if (PreFightManager.m_P2FighterArray[0] != "")
                {
                    // ele ja esta selecionando a skin, ou seja: trocar pra escolha de fighter
                    PreFightManager.m_P2FighterArray[0] = "";
                    m_SkinSelectGO.SetActive(false);
                    m_ES.SetSelectedGameObject(m_FigSelP2);
                }
            }
        }
    }
}
