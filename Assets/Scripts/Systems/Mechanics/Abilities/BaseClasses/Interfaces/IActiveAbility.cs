using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActiveAbility
{
    public float CalculateAbilityCooldown();
    public bool AbilityCastInput();
    public bool CanCastAbility();
}
