using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoCinematicUI : MonoBehaviour
{
    public static VideoCinematicUI Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<VideoCinematicTransitionTypeAnimator> transitionTypeAnimators;

    [Header("Components")]
    [SerializeField] private Button skipCinematicButton;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const string HIDDEN_ANIMATION_NAME = "Hidden";

    private const string TRANSITION_IN_OPENING_ANIMATION_NAME = "TransitionInOpening";
    private const string TRANSITION_IN_CLOSING_ANIMATION_NAME = "TransitionInClosing";

    private const string TRANSITION_OUT_OPENING_ANIMATION_NAME = "TransitionOutOpening";
    private const string TRANSITION_OUT_CLOSING_ANIMATION_NAME = "TransitionOutClosing";

    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInOpeningStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInOpeningEnd;

    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInClosingStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInClosingEnd;

    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutOpeningStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutOpeningEnd;

    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutClosingStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutClosingEnd;

    private VideoCinematicSO currentVideoCinematic;

    public class OnVideoCinematicUIEventArgs : EventArgs
    {
        public VideoCinematicSO videoCinematicSO;
    }

    [Serializable]
    public class VideoCinematicTransitionTypeAnimator
    {
        public VideoCinematicTransitionType transitionType;
        public Animator animator;
    }

    private void OnEnable()
    {
        VideoCinematicManager.OnCinematicBeginA += VideoCinematicManager_OnCinematicBeginA;
        VideoCinematicManager.OnCinematicBeginB += VideoCinematicManager_OnCinematicBeginB;

        VideoCinematicManager.OnCinematicEndA += VideoCinematicManager_OnCinematicEndA;
        VideoCinematicManager.OnCinematicEndB += VideoCinematicManager_OnCinematicEndB;

        VideoCinematicManager.OnCinematicIdle += VideoCinematicManager_OnCinematicIdle;
        VideoCinematicManager.OnGeneralCinematicConcluded += VideoCinematicManager_OnGeneralCinematicConcluded;
    }

    private void OnDisable()
    {
        VideoCinematicManager.OnCinematicBeginA -= VideoCinematicManager_OnCinematicBeginA;
        VideoCinematicManager.OnCinematicBeginB -= VideoCinematicManager_OnCinematicBeginB;

        VideoCinematicManager.OnCinematicEndA -= VideoCinematicManager_OnCinematicEndA;
        VideoCinematicManager.OnCinematicEndB -= VideoCinematicManager_OnCinematicEndB;

        VideoCinematicManager.OnCinematicIdle -= VideoCinematicManager_OnCinematicIdle;
        VideoCinematicManager.OnGeneralCinematicConcluded -= VideoCinematicManager_OnGeneralCinematicConcluded;
    }

    private void Awake()
    {
        SetSingleton();
        IntializeButtonsListeners();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one VideoCinematicUI instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void IntializeButtonsListeners()
    {
        skipCinematicButton.onClick.AddListener(SkipCinematic);
    }

    private void SetCurrentVideoCinematic(VideoCinematicSO videoCinematicSO) => currentVideoCinematic = videoCinematicSO;

    #region Animations
    private void CinematicTransitionInOpening(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_IN_OPENING_ANIMATION_NAME); 
        OnTransitionInOpeningStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
    }

    private void CinematicTransitionInClosing(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_IN_CLOSING_ANIMATION_NAME);
        OnTransitionInClosingStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
    }

    private void CinematicTransitionOutOpening(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_OUT_OPENING_ANIMATION_NAME);
        OnTransitionOutOpeningStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
    }

    private void CinematicTransitionOutClosing(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_OUT_CLOSING_ANIMATION_NAME);
        OnTransitionOutClosingStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
    }

    private void TransitionHidden(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(HIDDEN_ANIMATION_NAME);
    }
    #endregion

    public void TransitionInOpeningEnd() => OnTransitionInOpeningEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });
    public void TransitionInClosingEnd() => OnTransitionInClosingEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });
    public void TransitionOutOpeningEnd() => OnTransitionOutOpeningEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });
    public void TransitionOutClosingEnd() => OnTransitionOutClosingEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });

    private void SkipCinematic() => VideoCinematicManager.Instance.SkipCinematic();

    #region Duration Methods
    public float GetTransitionInClosingDurationForTransitionType(VideoCinematicTransitionType transitionType)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(transitionType);

        if(transitionAnimator == null)
        {
            if (debug) Debug.Log($"Transition Animation for TransitionType:{transitionType} is null. Returninf 0");
            return 0f;
        }

        return AnimationUtilities.GetAnimationClipDuration(transitionAnimator, TRANSITION_IN_CLOSING_ANIMATION_NAME);
    }

    public float GetTransitionOutOpeningDurationForTransitionType(VideoCinematicTransitionType transitionType)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(transitionType);

        if (transitionAnimator == null)
        {
            if (debug) Debug.Log($"Transition Animation for TransitionType:{transitionType} is null. Returninf 0");
            return 0f;
        }

        return AnimationUtilities.GetAnimationClipDuration(transitionAnimator, TRANSITION_OUT_OPENING_ANIMATION_NAME);
    }
    #endregion

    private Animator FindAnimatorByTransitionType(VideoCinematicTransitionType transitionType)
    {
        foreach (VideoCinematicTransitionTypeAnimator transitionTypeAnimator in transitionTypeAnimators)
        {
            if (transitionTypeAnimator.transitionType == transitionType) return transitionTypeAnimator.animator;
        }

        if (debug) Debug.Log($"Could not find animator for TransitionType: {transitionType}. Returning null Animator.");
        return null;

    }

    #region Subscriptions
    private void VideoCinematicManager_OnCinematicBeginA(object sender, VideoCinematicManager.OnVideoCinematicEventArgs e)
    {
        CinematicTransitionInOpening(e.videoCinematicSO);
    }
    private void VideoCinematicManager_OnCinematicBeginB(object sender, VideoCinematicManager.OnVideoCinematicEventArgs e)
    {
        CinematicTransitionInClosing(e.videoCinematicSO);
    }
    private void VideoCinematicManager_OnCinematicEndA(object sender, VideoCinematicManager.OnVideoCinematicEventArgs e)
    {
        CinematicTransitionOutOpening(e.videoCinematicSO);
    }
    private void VideoCinematicManager_OnCinematicEndB(object sender, VideoCinematicManager.OnVideoCinematicEventArgs e)
    {
        CinematicTransitionOutClosing(e.videoCinematicSO);
    }

    private void VideoCinematicManager_OnCinematicIdle(object sender, VideoCinematicManager.OnVideoCinematicEventArgs e)
    {
        TransitionHidden(e.videoCinematicSO);
    }

    private void VideoCinematicManager_OnGeneralCinematicConcluded(object sender, VideoCinematicManager.OnVideoCinematicEventArgs e)
    {
        TransitionHidden(e.videoCinematicSO);
    }
    #endregion

}
