using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataModeledCharacterData
{
    public int characterID;
    [Space]
    public int runsPlayed;
    public int runsWon;
    public int runsLost;
    [Space]
    public List<int> dialoguesPlayedIDs;

    public DataModeledCharacterData(int characterID)
    {
        this.characterID = characterID;
        runsPlayed = 0;
        runsWon = 0;
        runsLost = 0;
        dialoguesPlayedIDs = new List<int>();
    }
}
