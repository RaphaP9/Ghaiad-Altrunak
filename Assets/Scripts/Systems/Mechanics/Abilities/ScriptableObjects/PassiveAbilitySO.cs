using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbilitySO : AbilitySO, IPassiveAbilitySO
{
    public override AbilityType GetAbilityType() => AbilityType.Passive;

}
