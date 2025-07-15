using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliceAttack : PlayerProjectileAttack
{
    [Header("Staccato Settings")]
    [SerializeField] private Staccato staccato;

    [Header("Specific Components")]
    [SerializeField] private Transform secondaryProjectilePrefab;
    [Space]
    [SerializeField] private Transform secondaryFirePoint1;
    [SerializeField] private Transform secondaryFirePoint2;

    public event EventHandler<OnDeliceAttackEventArgs> OnDeliceRegularAttack;
    public static event EventHandler<OnDeliceAttackEventArgs> OnAnyDeliceRegularAttack;

    public event EventHandler<OnDeliceAttackEventArgs> OnDeliceBurstAttack;
    public static event EventHandler<OnDeliceAttackEventArgs> OnAnyDeliceBurstAttack;

    public class OnDeliceAttackEventArgs : EventArgs
    {
        public AbilityLevel staccatoLevel;
        public bool isSecondary;
    }

    protected override void Attack()
    {
        if (staccato.IsCurrentlyActive)
        {
            HandleStaccatoAttack();
        }
        else
        {
            HandleRegularAttack();
        }
    }

    private void HandleRegularAttack()
    {
        switch (staccato.AbilityLevel)
        {
            case AbilityLevel.NotLearned:
            case AbilityLevel.Level1:
            default:
                BasicAttack();
                break;
            case AbilityLevel.Level2:
            case AbilityLevel.Level3:
                StartCoroutine(ComposedAttackCoroutine());
                break;
        }
    }

    private void HandleStaccatoAttack()
    {
        switch (staccato.AbilityLevel)
        {
            case AbilityLevel.NotLearned:
            default:
                break;
            case AbilityLevel.Level1:
            case AbilityLevel.Level2:
                StartCoroutine(StaccatoBasicAttackCoroutine());
                break;
            case AbilityLevel.Level3:
                StartCoroutine(StaccatoComposedAttackCoroutine());
                break;

        }
    }

    private void BasicAttack()
    {
        ShootProjectile(mainProjectilePrefab, mainFirePoint, directionerHandler.GetDirection());

        OnDeliceRegularAttack?.Invoke(this, new OnDeliceAttackEventArgs { staccatoLevel = staccato.AbilityLevel, isSecondary = false });
        OnAnyDeliceRegularAttack?.Invoke(this, new OnDeliceAttackEventArgs { staccatoLevel = staccato.AbilityLevel , isSecondary = false });
    }

    private IEnumerator ComposedAttackCoroutine()
    {
        ShootProjectile(mainProjectilePrefab, mainFirePoint, directionerHandler.GetDirection());

        OnDeliceRegularAttack?.Invoke(this, new OnDeliceAttackEventArgs { staccatoLevel = staccato.AbilityLevel, isSecondary = false });
        OnAnyDeliceRegularAttack?.Invoke(this, new OnDeliceAttackEventArgs { staccatoLevel = staccato.AbilityLevel, isSecondary = false });

        yield return new WaitForSeconds(staccato.GetSecondaryAttackInterval());

        ShootProjectile(secondaryProjectilePrefab, mainFirePoint, directionerHandler.GetDirection(), staccato.GetSecondaryAttackDamagePercentage());

        OnDeliceRegularAttack?.Invoke(this, new OnDeliceAttackEventArgs { staccatoLevel = staccato.AbilityLevel, isSecondary = false });
        OnAnyDeliceRegularAttack?.Invoke(this, new OnDeliceAttackEventArgs { staccatoLevel = staccato.AbilityLevel, isSecondary = false });
    }

    private IEnumerator StaccatoBasicAttackCoroutine()
    {
        for(int i=0; i < staccato.GetBurstCount(); i++)
        {
            ShootProjectile(mainProjectilePrefab, mainFirePoint, directionerHandler.GetDirection());

            OnDeliceBurstAttack?.Invoke(this, new OnDeliceAttackEventArgs {staccatoLevel = staccato.AbilityLevel });
            OnAnyDeliceBurstAttack?.Invoke(this, new OnDeliceAttackEventArgs { staccatoLevel = staccato.AbilityLevel });

            yield return new WaitForSeconds(staccato.GetBurstInterval());
        }
    }

    private IEnumerator StaccatoComposedAttackCoroutine()
    {
        for (int i = 0; i < staccato.GetBurstCount(); i++)
        {
            Vector2 direction = directionerHandler.GetDirection();

            float secondaryDeviation1 = staccato.GetSecondaryBurstAngleDeviation();
            float secondaryDeviation2 = -staccato.GetSecondaryBurstAngleDeviation();

            if(direction.x < 0) //Take in count firepoints flipped on Y (Scale Flipped when facing left)
            {
                secondaryDeviation1 *= -1f;
                secondaryDeviation2 *= -1f;
            }

            ShootProjectile(mainProjectilePrefab, mainFirePoint, direction);
            ShootProjectile(secondaryProjectilePrefab, secondaryFirePoint1, GeneralUtilities.RotateVector2ByAngleDegrees(direction, secondaryDeviation1));
            ShootProjectile(secondaryProjectilePrefab, secondaryFirePoint2, GeneralUtilities.RotateVector2ByAngleDegrees(direction, secondaryDeviation2));

            OnDeliceBurstAttack?.Invoke(this, new OnDeliceAttackEventArgs { staccatoLevel = staccato.AbilityLevel });
            OnAnyDeliceBurstAttack?.Invoke(this, new OnDeliceAttackEventArgs { staccatoLevel = staccato.AbilityLevel });

            yield return new WaitForSeconds(staccato.GetBurstInterval());
        }
    }
}
