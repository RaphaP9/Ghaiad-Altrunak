using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IHasHealth 
{
    public bool AvoidDamageTakeHits(); //Avoid all damage and get hit by projectiles
    public bool AvoidDamagePassThrough(); //Avoid all damage and pass through projectiles
    public bool CanHeal();
    public bool CanRestoreShield();

    public bool TakeDamage(DamageData damageData); //True when attack landed/projectile impacted, false otherwise (Is not alive, Can avoid damage or has dodged)
    public void Heal(HealData healData);
    public void RestoreShield(ShieldData shieldData);

    public void Execute(ExecuteDamageData executeDamageData);
    public void HealCompletely(IHealSource healSource);
    public void RestoreShieldCompletely(IShieldSource shieldSource);

    public bool HasShield();
    public bool IsAlive();
    public bool IsFullHealth();
    public bool IsFullShield();
}


