using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoundGroup
{
    public DifficultyTier difficultyTier;
    public List<RoundSO> rounds;

    public RoundSO GetRandomRoundFromRoundsList()
    {
        if (rounds.Count <= 0)
        {
            Debug.Log("Round List has no elements. Returning null.");
            return null;
        }

        if (rounds.Count == 1) return rounds[0];

        RoundSO randomRound = GeneralUtilities.ChooseRandomElementFromList<RoundSO>(rounds);
        return randomRound;
    }

    public int GetRoundsCount() => rounds.Count;
}
