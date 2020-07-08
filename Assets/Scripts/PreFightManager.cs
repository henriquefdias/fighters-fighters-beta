using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreFightManager : MonoBehaviour
{
    // ORDEM DO VETOR: Nome, Roupa, Is Player, Is P1

    public static string[] m_P1FighterArray = { "", "", "", bool.TrueString };
    public static string[] m_P2FighterArray = { "", "", "", bool.FalseString };
    public static bool m_IsVsCPU;
    public static bool m_IsP1Choosing;
    public static int m_ArenaIndex;
    public static GameObject[] m_FightersPrefabs;
    public GameObject[] m_FightersPrefabsNotStatic;
    public static Sprite[] m_ArenaBackgrounds;
    public Sprite[] m_ArenaBackgroundsNotStatic;
    public static Material[] m_ArenaGrounds;
    public Material[] m_ArenaGroundsNotStatic;
    public static Material[] m_SkinArray;
    public Material[] m_SkinArrayNotStatic;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        m_IsP1Choosing = true;
        m_FightersPrefabs = m_FightersPrefabsNotStatic; // TODO: melhorar essa parte
        m_ArenaBackgrounds = m_ArenaBackgroundsNotStatic;
        m_ArenaGrounds = m_ArenaGroundsNotStatic;
        m_SkinArray = m_SkinArrayNotStatic;
    }

    public void SetVsCPU(bool value)
    {
        m_IsVsCPU = value;
    }

    public void FeedInfo(string info, string value = "")
    {
        if (info == "isPlayer")
        {
            if (m_IsP1Choosing)
                m_P1FighterArray[1] = bool.TrueString;
            else
            {
                if (m_IsVsCPU)
                    m_P2FighterArray[1] = bool.FalseString;
                else
                    m_P2FighterArray[1] = bool.TrueString;
            }
        }

        if (info == "isP1")
        {
            if (m_IsP1Choosing)
                m_P1FighterArray[2] = bool.TrueString;
            else
                m_P2FighterArray[2] = bool.FalseString;
        }
    }

    public void FeedIsP2Choosing()
    {
        m_IsP1Choosing = false;
    }
}
