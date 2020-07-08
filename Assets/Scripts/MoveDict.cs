using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDict : MonoBehaviour
{
    private Dictionary<string, string[]> m_MovesDict = new Dictionary<string, string[]>();
    void Start()
    {
        FillMovesDict();
    }

    public string[] SearchMove(string input)
    {
        string[] outValue = null;
        m_MovesDict.TryGetValue(input, out outValue);
        return outValue;
    }

    private void FillMovesDict()
    {
        m_MovesDict.Add("S1", new string[3] {"Soco1", "5", "0"});
        m_MovesDict.Add("S2", new string[3] {"Soco2", "7", "0"});
        m_MovesDict.Add("C1", new string[3] {"Chute1", "6", "0"});
        m_MovesDict.Add("C2", new string[3] {"Chute2", "8", "0"});
        //m_MovesDict.Add("BFS1", new string[3] {"Hadouken", "10", "0"});
        //m_MovesDict.Add("TFS2", new string[3] {"Sonic Boom", "15", "0"});
        //m_MovesDict.Add("BTC1", new string[3] {"Taregueteguetuguen", "15", "0"});
        //m_MovesDict.Add("BBC2", new string[3] {"Rasteira", "18", "0"});
        //m_MovesDict.Add("BFBFS1", new string[3] {"COSMÓPOLIS SMASH", "30", "50"});
        //m_MovesDict.Add("FBTBFC2", new string[3] {"SUMARÉ SMASH", "45", "50"});
        // TODO: na versão final, contemplar golpes especiais
    }
}
