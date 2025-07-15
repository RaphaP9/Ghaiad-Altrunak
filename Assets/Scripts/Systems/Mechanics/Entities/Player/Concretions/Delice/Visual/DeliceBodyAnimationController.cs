using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliceBodyAnimationController : CharacterBodyAnimationController
{
    [Header("Specific Components")]
    [SerializeField] private Staccato staccato;
    [SerializeField] private Ritardando ritardando;
    [SerializeField] private Legato legato;

    private const string STACCATO_PERFORMANCE_ANIMATION_NAME = "StaccatoPerformance";
    private const string RITARDANDO_PERFORMANCE_ANIMATION_NAME = "RitardandoPerformance";

    private const string LEGATO_IN_ANIMATION_NAME = "LegatoIn";
    private const string LEGATO_BLEND_TREE_NAME = "LegatoBlendTree";
    private const string LEGATO_OUT_ANIMATION_NAME = "LegatoOut";

    private bool onLegato = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        staccato.OnStaccatoPerformanceStart += Staccato_OnStaccatoPerformanceStart;
        staccato.OnStaccatoPerformanceEnd += Staccato_OnStaccatoPerformanceEnd;

        ritardando.OnRitardandoPerformanceStart += Ritardando_OnRitardandoPerformanceStart;
        ritardando.OnRitardandoPerformanceEnd += Ritardando_OnRitardandoPerformanceEnd;

        legato.OnLegatoStarting += Legato_OnLegatoStarting;
        legato.OnLegatoStart += Legato_OnLegatoStart;
        legato.OnLegatoEnding += Legato_OnLegatoEnding;
        legato.OnLegatoCompleted += Legato_OnLegatoCompleted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        staccato.OnStaccatoPerformanceStart -= Staccato_OnStaccatoPerformanceStart;
        staccato.OnStaccatoPerformanceEnd -= Staccato_OnStaccatoPerformanceEnd;

        ritardando.OnRitardandoPerformanceStart -= Ritardando_OnRitardandoPerformanceStart;
        ritardando.OnRitardandoPerformanceEnd -= Ritardando_OnRitardandoPerformanceEnd;

        legato.OnLegatoStarting -= Legato_OnLegatoStarting;
        legato.OnLegatoStart -= Legato_OnLegatoStart;
        legato.OnLegatoEnding -= Legato_OnLegatoEnding;
        legato.OnLegatoCompleted -= Legato_OnLegatoCompleted;
    }

    #region Subscriptions
    private void Staccato_OnStaccatoPerformanceStart(object sender, System.EventArgs e)
    {
        if (onLegato) return;
        PlayAnimation(STACCATO_PERFORMANCE_ANIMATION_NAME);
    }

    private void Staccato_OnStaccatoPerformanceEnd(object sender, System.EventArgs e)
    {
        if (onLegato) return;
        PlayAnimation(MOVEMENT_BLEND_TREE_NAME);
    }

    private void Ritardando_OnRitardandoPerformanceStart(object sender, System.EventArgs e)
    {
        if (onLegato) return;
        PlayAnimation(RITARDANDO_PERFORMANCE_ANIMATION_NAME);
    }
    private void Ritardando_OnRitardandoPerformanceEnd(object sender, System.EventArgs e)
    {
        if (onLegato) return;
        PlayAnimation(MOVEMENT_BLEND_TREE_NAME);
    }

    private void Legato_OnLegatoStarting(object sender, System.EventArgs e)
    {
        onLegato = true;
        PlayAnimation(LEGATO_IN_ANIMATION_NAME);
    }

    private void Legato_OnLegatoStart(object sender, System.EventArgs e)
    {
        PlayAnimation(LEGATO_BLEND_TREE_NAME);
    }

    private void Legato_OnLegatoEnding(object sender, System.EventArgs e)
    {
        onLegato = false;
        PlayAnimation(LEGATO_OUT_ANIMATION_NAME);
    }

    private void Legato_OnLegatoCompleted(object sender, System.EventArgs e)
    {
        PlayAnimation(MOVEMENT_BLEND_TREE_NAME);
    }
    #endregion
}
