using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionUIHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<TransitionTypeAnimator> transitionTypeAnimators;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const string TRANSITION_OUT_TRIGGER = "TransitionOut";
    private const string TRANSITION_IN_TRIGGER = "TransitionIn";

    private const string IDLE_ANIMATION_NAME = "Idle";
    private const string MIDDLE_TRANSITION_ANIMATION_NAME = "MiddleTransition";

    public static event EventHandler<OnTransitionUIEventArgs> OnTransitionOutEnd;
    public static event EventHandler<OnTransitionUIEventArgs> OnTransitionInEnd;

    private TransitionType currentTransitionType;

    public class OnTransitionUIEventArgs : EventArgs
    {
        public TransitionType transitionType;
    }

    [Serializable]
    public class TransitionTypeAnimator
    {
        public TransitionType transitionType;
        public Animator animator;
    }

    private void OnEnable()
    {
        ScenesManager.OnSceneTransitionOutStart += ScenesManager_OnSceneTransitionOutStart;
        ScenesManager.OnSceneTransitionInStart += ScenesManager_OnSceneTransitionInStart;
    }

    private void OnDisable()
    {
        ScenesManager.OnSceneTransitionOutStart -= ScenesManager_OnSceneTransitionOutStart;
        ScenesManager.OnSceneTransitionInStart -= ScenesManager_OnSceneTransitionInStart;
    }

    private void SetCurrentTransitionType(TransitionType transitionType) => currentTransitionType = transitionType;    

    private void TriggerTransitionOut(TransitionType transitionType)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(IDLE_ANIMATION_NAME); //Always start transition out from Idle

        transitionAnimator.ResetTrigger(TRANSITION_IN_TRIGGER);
        transitionAnimator.SetTrigger(TRANSITION_OUT_TRIGGER);
    }

    private void TriggerTransitionIn(TransitionType transitionType)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(MIDDLE_TRANSITION_ANIMATION_NAME); //Always start transition out from MiddleTransition

        transitionAnimator.ResetTrigger(TRANSITION_OUT_TRIGGER);
        transitionAnimator.SetTrigger(TRANSITION_IN_TRIGGER);
    }

    public void TransitionInEnd() => OnTransitionInEnd?.Invoke(this, new OnTransitionUIEventArgs { transitionType = currentTransitionType});
    public void TransitionOutEnd() => OnTransitionOutEnd?.Invoke(this, new OnTransitionUIEventArgs { transitionType = currentTransitionType });

    private Animator FindAnimatorByTransitionType(TransitionType transitionType)
    {
        foreach(TransitionTypeAnimator transitionTypeAnimator in transitionTypeAnimators)
        {
            if (transitionTypeAnimator.transitionType == transitionType) return transitionTypeAnimator.animator;
        }

        if (debug) Debug.Log($"Could not find animator for TransitionType: {transitionType}. Returning null Animator.");
        return null;

    }

    #region ScenesManager Subscriptions
    private void ScenesManager_OnSceneTransitionOutStart(object sender, ScenesManager.OnSceneTransitionLoadEventArgs e)
    {
        SetCurrentTransitionType(e.transitionType);
        TriggerTransitionOut(e.transitionType);
    }

    private void ScenesManager_OnSceneTransitionInStart(object sender, ScenesManager.OnSceneTransitionLoadEventArgs e)
    {
        SetCurrentTransitionType(e.transitionType);
        TriggerTransitionIn(e.transitionType);
    }
    #endregion
}