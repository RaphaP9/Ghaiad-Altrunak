using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeCardsUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform abilityUpgradeCardsContainer;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        AbilityUpgradeManager.OnAbilityUpgradesGenerated += AbilityUpgradeManager_OnAbilityUpgradesGenerated;
    }

    private void OnDisable()
    {
        AbilityUpgradeManager.OnAbilityUpgradesGenerated -= AbilityUpgradeManager_OnAbilityUpgradesGenerated;
    }

    private void GenerateNewAbilityUpgradeCards(List<AbilityUpgradeCardInfo> abilityUpgradeCardInfos)
    {
        ClearAbilityUpgradeCardsContainer();

        foreach (AbilityUpgradeCardInfo abilityUpgradeCardInfo in abilityUpgradeCardInfos)
        {
            CreateAbilityUpgradeCard(abilityUpgradeCardInfo);
        }
    }

    private void ClearAbilityUpgradeCardsContainer()
    {
        foreach (Transform child in abilityUpgradeCardsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateAbilityUpgradeCard(AbilityUpgradeCardInfo abilityUpgradeCardInfo)
    {
        AbilitySO abilitySO = abilityUpgradeCardInfo.abilitySO;
        AbilityLevelInfo abilityLevelInfo = abilitySO.abilityInfoSO.GetAbilityLevelInfoByLevel(abilityUpgradeCardInfo.upgradeLevel);

        if(abilityLevelInfo == null)
        {
            if (debug) Debug.Log($"Ability Level Info of Ability: {abilityUpgradeCardInfo.abilitySO.name} and Uprade Level: {abilityUpgradeCardInfo.upgradeLevel} does not exist. Can not instantiate card.");
            return;
        }

        Transform abilityUpgradeCardUITransform = Instantiate(abilityLevelInfo.cardPrefab, abilityUpgradeCardsContainer);

        AbilityUpgradeCardUI abilityUpgradeCardUI = abilityUpgradeCardUITransform.GetComponent<AbilityUpgradeCardUI>();

        if (abilityUpgradeCardUI == null)
        {
            if (debug) Debug.Log("Instantiated Ability Upgrade Card does not contain a AbilityUpgradeCardUI component. Set will be ignored.");
            return;
        }

        abilityUpgradeCardUI.SetAbilityUpgradeCardInfo(abilityUpgradeCardInfo, abilityLevelInfo);
    }

    private void AbilityUpgradeManager_OnAbilityUpgradesGenerated(object sender, AbilityUpgradeManager.OnAbilityUpgradesEventArgs e)
    {
        GenerateNewAbilityUpgradeCards(e.abilityUpgradeCardInfos);
    }
}
