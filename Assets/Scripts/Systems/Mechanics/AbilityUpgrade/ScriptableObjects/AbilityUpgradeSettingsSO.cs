using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityUpgradeSettingsSO", menuName = "ScriptableObjects/AbilityUpgrades/AbilityUpgradeSettings")]

public class AbilityUpgradeSettingsSO : ScriptableObject
{
    [Header("Ability Upgrade Size")]
    [Range(1, 3)] public int maxUpgrades;
}
