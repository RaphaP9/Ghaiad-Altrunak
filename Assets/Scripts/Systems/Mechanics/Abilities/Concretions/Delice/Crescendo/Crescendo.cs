using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crescendo : PassiveAbility
{
    [Header("Specific Runtime Filled")]
    [SerializeField] private string activeAbilityGUID;

    private CrescendoSO CrescendoSO => AbilitySO as CrescendoSO;

    public static event EventHandler OnAnyCrescendoTrigger;
    public event EventHandler OnCrescendoTrigger;

    public float CrescendoTimer { get; private set; }
    public float BonificationDuration => GetBonificationDuration();
    private bool isCurrentlyActive = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        Ability.OnAnyAbilityCast += Ability_OnAnyAbilityCast;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Ability.OnAnyAbilityCast -= Ability_OnAnyAbilityCast;
    }

    #region Logic Methods
    protected override void HandleFixedUpdateLogic() { }
    protected override void HandleUpdateLogic() { }
    #endregion

    private void HandleCrescendoTrigger()
    {
        if (!IsActiveVariant) return;

        //If is currently active, CrescendoCoroutine is happening, we can just reset the CrescendoTimer as we only want one Crescendo active at once
        if (isCurrentlyActive) 
        {
            ResetTimer();
            return;
        }

        StartCoroutine(CrescendoCoroutine());
    }

    private IEnumerator CrescendoCoroutine()
    {
        isCurrentlyActive = true;

        OnAnyCrescendoTrigger?.Invoke(this, EventArgs.Empty);
        OnCrescendoTrigger?.Invoke(this, EventArgs.Empty);

        string generatedGUID = GeneralUtilities.GenerateGUID();
        SetAbilityGUID(generatedGUID);
        TemporalNumericStatModifierManager.Instance.AddStatModifiers(generatedGUID, CrescendoSO);

        ResetTimer();

        while (CrescendoTimer < GetBonificationDuration())
        {
            CrescendoTimer += Time.deltaTime;
            yield return null;
        }

        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(generatedGUID);
        ClearAbilityGUID();

        isCurrentlyActive = false;
    }

    private float GetBonificationDuration()
    {
        return CrescendoSO.bonificationDuration;
    }

    private void SetAbilityGUID(string GUID) => activeAbilityGUID = GUID;
    private void ClearAbilityGUID() => activeAbilityGUID = "";
    private void ResetTimer() => CrescendoTimer = 0;

    private void Ability_OnAnyAbilityCast(object sender, OnAbilityCastEventArgs e)
    {
        HandleCrescendoTrigger();
    }
}
