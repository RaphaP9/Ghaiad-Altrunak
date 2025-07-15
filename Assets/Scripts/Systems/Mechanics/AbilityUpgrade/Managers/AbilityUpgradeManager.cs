using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeManager : MonoBehaviour
{
    public static AbilityUpgradeManager Instance { get; private set; }

    public static event EventHandler<OnAbilityUpgradesEventArgs> OnAbilityUpgradesGenerated;

    public class OnAbilityUpgradesEventArgs : EventArgs
    {
        public List<AbilityUpgradeCardInfo> abilityUpgradeCardInfos;
    }

    private void OnEnable()
    {
        AbilityUpgradeOpeningManager.OnAbilityUpgradeOpen += AbilityUpgradeOpeningManager_OnAbilityUpgradeOpen;
        AbilityUpgradeCardPressingHandler.OnAnyAbilityUpgradeCardPressed += AbilityUpgradeCardPressingHandler_OnAnyAbilityUpgradeCardPressed;
    }

    private void OnDisable()
    {
        AbilityUpgradeOpeningManager.OnAbilityUpgradeOpen -= AbilityUpgradeOpeningManager_OnAbilityUpgradeOpen;
        AbilityUpgradeCardPressingHandler.OnAnyAbilityUpgradeCardPressed -= AbilityUpgradeCardPressingHandler_OnAnyAbilityUpgradeCardPressed;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void GenerateAbilityUpgrades()
    {
        List<AbilityUpgradeCardInfo> abilityUpgradeCardInfos = AbilityUpgradeCardsGenerator.Instance.GenerateNextLevelActiveAbilityVariantCards();
        OnAbilityUpgradesGenerated?.Invoke(this, new OnAbilityUpgradesEventArgs { abilityUpgradeCardInfos = abilityUpgradeCardInfos });
    } 


    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AbilityUpgradeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void AbilityUpgradeOpeningManager_OnAbilityUpgradeOpen(object sender, EventArgs e)
    {
        GenerateAbilityUpgrades();
    }

    private void AbilityUpgradeCardPressingHandler_OnAnyAbilityUpgradeCardPressed(object sender, AbilityUpgradeCardPressingHandler.OnAbilityUpgradeCardEventArgs e)
    {
        AbilityUpgradeOpeningManager.Instance.CloseAbilityUpgrade();
    }
}
