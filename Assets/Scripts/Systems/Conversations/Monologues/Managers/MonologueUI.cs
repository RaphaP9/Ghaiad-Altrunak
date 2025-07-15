using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonologueUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI sentenceText;

    [Header("Runtime Filled")]
    [SerializeField] private MonologueSentence currentMonologueSentence;

    #region Animation Names
    private const string HIDDEN_ANIMATION_NAME = "Hidden";
    private const string IDLE_ANIMATION_NAME = "Idle";

    private const string MONOLOGUE_TRANSITION_IN_ANIMATION_NAME = "MonologueTransitionIn";
    private const string MONOLOGUE_TRANSITION_OUT_ANIMATION_NAME = "MonologueTransitionOut";

    private const string SENTENCE_TRANSITION_IN_ANIMATION_NAME = "SentenceTransitionIn";
    private const string SENTENCE_TRANSITION_OUT_ANIMATION_NAME = "SentenceTransitionOut";
    #endregion

    #region Events
    public static event EventHandler<OnMonologueSentenceEventArgs> OnMonologueTransitionInStart;
    public static event EventHandler<OnMonologueSentenceEventArgs> OnMonologueTransitionInEnd;

    public static event EventHandler<OnMonologueSentenceEventArgs> OnSentenceTransitionInStart;
    public static event EventHandler<OnMonologueSentenceEventArgs> OnSentenceTransitionInEnd;

    public static event EventHandler<OnMonologueSentenceEventArgs> OnSentenceTransitionOutStart;
    public static event EventHandler<OnMonologueSentenceEventArgs> OnSentenceTransitionOutEnd;

    public static event EventHandler<OnMonologueSentenceEventArgs> OnMonologueTransitionOutStart;
    public static event EventHandler<OnMonologueSentenceEventArgs> OnMonologueTransitionOutEnd;
    #endregion

    public class OnMonologueSentenceEventArgs : EventArgs
    {
        public MonologueSentence monologueSentence;
    }

    private void OnEnable()
    {
        MonologueManager.OnMonologueBegin += MonologueManager_OnMonologueBegin;
        MonologueManager.OnMonologueEnd += MonologueManager_OnMonologueEnd;
        MonologueManager.OnSentenceBegin += MonologueManager_OnSentenceBegin;
        MonologueManager.OnSentenceEnd += MonologueManager_OnSentenceEnd;
        MonologueManager.OnSentenceIdle += MonologueManager_OnSentenceIdle;
        MonologueManager.OnGeneralMonologueConcluded += MonologueManager_OnGeneralMonologueConcluded;
    }

    private void OnDisable()
    {
        MonologueManager.OnMonologueBegin -= MonologueManager_OnMonologueBegin;
        MonologueManager.OnMonologueEnd -= MonologueManager_OnMonologueEnd;
        MonologueManager.OnSentenceBegin -= MonologueManager_OnSentenceBegin;
        MonologueManager.OnSentenceEnd -= MonologueManager_OnSentenceEnd;
        MonologueManager.OnSentenceIdle -= MonologueManager_OnSentenceIdle;
        MonologueManager.OnGeneralMonologueConcluded -= MonologueManager_OnGeneralMonologueConcluded;
    }

    private void PlayAnimation(string animationName) => animator.Play(animationName);

    #region Animations
    private void MonologueTransitionIn()
    {
        PlayAnimation(MONOLOGUE_TRANSITION_IN_ANIMATION_NAME);
        OnMonologueTransitionInStart?.Invoke(this, new OnMonologueSentenceEventArgs());
    }

    private void MonologueTransitionOut()
    {
        PlayAnimation(MONOLOGUE_TRANSITION_OUT_ANIMATION_NAME);
        OnMonologueTransitionOutStart?.Invoke(this, new OnMonologueSentenceEventArgs());
    }

    private void SentenceTransitionIn()
    {
        PlayAnimation(SENTENCE_TRANSITION_IN_ANIMATION_NAME);
        OnSentenceTransitionInStart?.Invoke(this, new OnMonologueSentenceEventArgs());
    }

    private void SentenceTransitionOut()
    {
        PlayAnimation(SENTENCE_TRANSITION_OUT_ANIMATION_NAME);
        OnSentenceTransitionOutStart?.Invoke(this, new OnMonologueSentenceEventArgs());
    }

    private void SentenceIdle()
    {
        PlayAnimation(IDLE_ANIMATION_NAME);
    }

    private void MonologueConcluded()
    {
        PlayAnimation(HIDDEN_ANIMATION_NAME);
    }
    #endregion

    #region Animation Event Methods
    public void TriggerMonologueTransitionInEnd() => OnMonologueTransitionInEnd?.Invoke(this, new OnMonologueSentenceEventArgs { });
    public void TriggerMonologueTransitionOutEnd() => OnMonologueTransitionOutEnd?.Invoke(this, new OnMonologueSentenceEventArgs { });
    public void TriggerSentenceTransitionInEnd() => OnSentenceTransitionInEnd?.Invoke(this, new OnMonologueSentenceEventArgs { });
    public void TriggerSentenceTransitionOutEnd() => OnSentenceTransitionOutEnd?.Invoke(this, new OnMonologueSentenceEventArgs { });
    #endregion

    #region Set 
    private void SetSentenceUI(MonologueSentence monologueSentence)
    {
        SetCurrentMonologueSentence(monologueSentence);

        sentenceText.text = monologueSentence.sentenceText;
        sentenceText.color = monologueSentence.monologueSpeakerSO.textColor;

        sentenceText.ForceMeshUpdate();
    }

    private void SetCurrentMonologueSentence(MonologueSentence monologueSentence) => currentMonologueSentence = monologueSentence;
    #endregion

    #region Subscriptions
    private void MonologueManager_OnMonologueBegin(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        SetSentenceUI(e.monologueSentence);
        MonologueTransitionIn();
    }

    private void MonologueManager_OnMonologueEnd(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        MonologueTransitionOut();
    }

    private void MonologueManager_OnSentenceBegin(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        SetSentenceUI(e.monologueSentence);
        SentenceTransitionIn();
    }

    private void MonologueManager_OnSentenceEnd(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        SentenceTransitionOut();
    }

    private void MonologueManager_OnSentenceIdle(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        SentenceIdle();
    }

    private void MonologueManager_OnGeneralMonologueConcluded(object sender, EventArgs e)
    {
        MonologueConcluded();
    }
    #endregion
}
