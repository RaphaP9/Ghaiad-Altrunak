using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeCardsGenerator : MonoBehaviour
{
    public static AbilityUpgradeCardsGenerator Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private AbilityUpgradeSettingsSO abilityUpgradeSettingsSO;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<AbilityUpgradeCardInfo> GenerateNextLevelActiveAbilityVariantCards()
    {
        List<AbilityUpgradeCardInfo> activeAbilityVariantCards = new List<AbilityUpgradeCardInfo>();

        if (CharacterAbilitySlotsRegister.Instance == null)
        {
            if (debug) Debug.Log("CharacterAbilitySlotsRegister instance is null. Returning empty list");
            return activeAbilityVariantCards;
        }

        foreach(AbilitySlotHandler abilitySlotHandler in CharacterAbilitySlotsRegister.Instance.AbilitySlotHandlers)
        {
            AbilityUpgradeCardInfo abilityUpgradeCardInfo = GenerateNextLevelCard(abilitySlotHandler.ActiveAbilityVariant);
            if (abilityUpgradeCardInfo == null) continue;

            activeAbilityVariantCards.Add(abilityUpgradeCardInfo);
        }

        activeAbilityVariantCards = ReduceCardsToSize(activeAbilityVariantCards);

        return activeAbilityVariantCards;
    }

    public bool CanGenerateNextLevelActiveAbilityVariantCards()
    {
        List<AbilityUpgradeCardInfo> abilityUpgradeCardInfos = GenerateNextLevelActiveAbilityVariantCards();

        if(abilityUpgradeCardInfos.Count <= 0) return false;
        return true;
    }

    private AbilityUpgradeCardInfo GenerateNextLevelCard(Ability ability)
    {
        if(ability == null)
        {
            if (debug) Debug.Log($"Ability is null.");
            return null;
        }

        if (ability.AbilityLevelHandler.IsMaxedOut())
        {
            if (debug) Debug.Log($"Ability with name: {ability.AbilitySO.abilityName} is maxed out.");
            return null;
        }

        AbilityLevel currentLevel = ability.AbilityLevelHandler.AbilityLevel;
        AbilityLevel upgradeLevel = MechanicsUtilities.GetNextAbilityLevel(currentLevel);
        AbilitySO abilitySO = ability.AbilitySO;

        AbilityUpgradeCardInfo abilityUpgradeCardInfo = new AbilityUpgradeCardInfo { currentLevel = currentLevel, upgradeLevel = upgradeLevel, abilitySO = abilitySO, ability = ability };
        return abilityUpgradeCardInfo;
    }

    private List<AbilityUpgradeCardInfo> ReduceCardsToSize(List<AbilityUpgradeCardInfo> abilityUpgradeCardInfos)
    {
        if (abilityUpgradeCardInfos.Count <= abilityUpgradeSettingsSO.maxUpgrades) return abilityUpgradeCardInfos;

        List<AbilityUpgradeCardInfo> shuffledCards = GeneralUtilities.FisherYatesShuffle(abilityUpgradeCardInfos);
        List<AbilityUpgradeCardInfo> shortenList = new List<AbilityUpgradeCardInfo>();

        foreach(AbilityUpgradeCardInfo shuffledCard in shuffledCards)
        {
            shortenList.Add(shuffledCard);

            if(shortenList.Count >= abilityUpgradeSettingsSO.maxUpgrades) return shortenList;
        }

        return shortenList; 
    }

}
