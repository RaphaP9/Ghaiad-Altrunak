using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHoverUIOpeningHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilityHoverUIHandler abilityHoverUIHandler;
    [SerializeField] private Animator animator;

    private const string HOVER_IN_TRIGGER = "HoverIn";
    private const string HOVER_OUT_TRIGGER = "HoverOut";

    private void OnEnable()
    {
        abilityHoverUIHandler.OnHoverOpening += AbilityHoverUIHandler_OnHoverOpening;
        abilityHoverUIHandler.OnHoverClosing += AbilityHoverUIHandler_OnHoverClosing;
    }

    private void OnDisable()
    {
        abilityHoverUIHandler.OnHoverOpening -= AbilityHoverUIHandler_OnHoverOpening;
        abilityHoverUIHandler.OnHoverClosing -= AbilityHoverUIHandler_OnHoverClosing;
    }

    private void HoverIn()
    {
        animator.ResetTrigger(HOVER_OUT_TRIGGER);
        animator.SetTrigger(HOVER_IN_TRIGGER);
    }

    private void HoverOut()
    {
        animator.ResetTrigger(HOVER_IN_TRIGGER);
        animator.SetTrigger(HOVER_OUT_TRIGGER);
    }

    private void AbilityHoverUIHandler_OnHoverOpening(object sender, AbilityHoverUIHandler.OnAbilityEventArgs e)
    {
        HoverIn();
    }

    private void AbilityHoverUIHandler_OnHoverClosing(object sender, AbilityHoverUIHandler.OnAbilityEventArgs e)
    {
        HoverOut();
    }
}
