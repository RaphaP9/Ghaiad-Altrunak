using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWeaponRenderingHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityHealth entityHealth;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Component> attackInterruptionComponents;

    private List<IAttackInterruption> attackInterruptions;
    private bool renderingWeapon = true;

    private void Awake()
    {
        GetAttackInterruptionInterfaces();
    }

    private void Update()
    {
        HandleWeaponRendering();
    }

    private void GetAttackInterruptionInterfaces()
    {
        attackInterruptions = GeneralUtilities.TryGetGenericsFromComponents<IAttackInterruption>(attackInterruptionComponents);
    }

    private void HandleWeaponRendering()
    {
        if (CanRenderWeapon())
        {
            CheckShouldRender();
        }

        if (!CanRenderWeapon())
        {
            CheckStopRender();
        }
    }

    private void CheckShouldRender()
    {
        if (renderingWeapon) return;

        spriteRenderer.enabled = true;

        renderingWeapon = true;
    }

    private void CheckStopRender()
    {
        if (!renderingWeapon) return;

        spriteRenderer.enabled = false;

        renderingWeapon = false;
    }


    protected virtual bool CanRenderWeapon()
    {
        if (!entityHealth.IsAlive()) return false;

        foreach (IAttackInterruption attackInterruptionAbility in attackInterruptions)
        {
            if (attackInterruptionAbility.IsInterruptingAttack()) return false;
        }

        return true;
    }

}
