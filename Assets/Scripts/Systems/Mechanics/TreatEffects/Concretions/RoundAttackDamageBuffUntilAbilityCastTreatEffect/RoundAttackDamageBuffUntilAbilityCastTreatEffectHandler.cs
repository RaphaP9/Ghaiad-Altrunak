using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundAttackDamageBuffUntilAbilityCastTreatEffectHandler : RoundBuffUntilActionTreatEffectHandler
{
    public static RoundAttackDamageBuffUntilAbilityCastTreatEffectHandler Instance { get; private set; }

    private RoundAttackDamageBuffUntilAbilityCastTreatEffectSO RoundAttackDamageBuffUntilAbilityCastTreatEffectSO => treatEffectSO as RoundAttackDamageBuffUntilAbilityCastTreatEffectSO;

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

    protected override string GetRefferencialGUID() => RoundAttackDamageBuffUntilAbilityCastTreatEffectSO.refferencialGUID;

    protected override void AddBuff()
    {
        base.AddBuff();
        TemporalNumericStatModifierManager.Instance.AddSingleNumericStatModifier(GetRefferencialGUID(), RoundAttackDamageBuffUntilAbilityCastTreatEffectSO.buffStat);
    }

    private void Ability_OnAnyAbilityCast(object sender, Ability.OnAbilityCastEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;

        if (buffActive) RemoveBuff();
    }
}
