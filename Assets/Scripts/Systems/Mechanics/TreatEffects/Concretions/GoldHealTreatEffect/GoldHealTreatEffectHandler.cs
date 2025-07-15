using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldHealTreatEffectHandler : TreatEffectHandler
{
    public static GoldHealTreatEffectHandler Instance { get; private set; }

    private GoldHealTreatEffectSO GoldHealTreatEffectSO => treatEffectSO as GoldHealTreatEffectSO;

    private PlayerHealth playerHealth;

    public static event EventHandler OnHealByTreat;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
        GoldCollection.OnAnyGoldCollected += GoldCollection_OnAnyGoldCollected;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
        GoldCollection.OnAnyGoldCollected -= GoldCollection_OnAnyGoldCollected;
    }

    protected override void SetSingleton()
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

    private void HandleHeal()
    {
        bool probability = MechanicsUtilities.GetProbability(GoldHealTreatEffectSO.healProbability);

        if (!probability) return;

        HealData healData = new HealData(GoldHealTreatEffectSO.healPerGold, GoldHealTreatEffectSO);
        playerHealth.Heal(healData);

        OnHealByTreat?.Invoke(this, EventArgs.Empty);
    }

    #region Subscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerHealth = e.playerTransform.GetComponentInChildren<PlayerHealth>();
    }

    private void GoldCollection_OnAnyGoldCollected(object sender, GoldCollection.OnGoldEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;
        if (GameManager.Instance.GameState != GameManager.State.Combat) return; //Must be in combat

        HandleHeal();
    }
    #endregion
}
