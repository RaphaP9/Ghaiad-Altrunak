using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHoverUIOpeningHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsHoverUIHandler statsHoverUIHandler;
    [SerializeField] private Animator animator;

    private const string HOVER_IN_TRIGGER = "HoverIn";
    private const string HOVER_OUT_TRIGGER = "HoverOut";

    private void OnEnable()
    {
        statsHoverUIHandler.OnHoverOpening += StatsHoverUIHandler_OnHoverOpening;
        statsHoverUIHandler.OnHoverClosing += StatsHoverUIHandler_OnHoverClosing;
    }

    private void OnDisable()
    {
        statsHoverUIHandler.OnHoverOpening -= StatsHoverUIHandler_OnHoverOpening;
        statsHoverUIHandler.OnHoverClosing -= StatsHoverUIHandler_OnHoverClosing;
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

    private void StatsHoverUIHandler_OnHoverOpening(object sender, StatsHoverUIHandler.OnStatInfoEventArgs e)
    {
        HoverIn();
    }

    private void StatsHoverUIHandler_OnHoverClosing(object sender, StatsHoverUIHandler.OnStatInfoEventArgs e)
    {
        HoverOut();
    }
}
