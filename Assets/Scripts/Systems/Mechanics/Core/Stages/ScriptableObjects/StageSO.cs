using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageSO", menuName = "ScriptableObjects/Core/Stage")]
public class StageSO : ScriptableObject
{
    public string stageName;
    public Color stageNameColor;
    [Space]
    public List<RoundGroup> roundGroups;

    public int GetRoundsQuantityInStage() => roundGroups.Count;
}
