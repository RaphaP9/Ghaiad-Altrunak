using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueManager : MonoBehaviour
{
    public static MonologueManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private MonologueSO currentMonologueSO;
    [SerializeField] private MonologueSentence currentSentence;

    [Header("States - Runtime Filled")]
    [SerializeField] private MonologueState monologueState;

    public MonologueState State => monologueState;

    public enum MonologueState { NotOnMonologue, MonologueTransitionIn, MonologueTransitionOut, Idle, SentenceTransitionIn, SentenceTransitionOut }

    #region Flags
    private bool monologueTransitionInCompleted = false;
    private bool monologueTransitionOutCompleted = false;

    private bool sentenceTransitionInCompleted = false;
    private bool sentenceTransitionOutCompleted = false;

    public bool shouldSkipSentence = false;
    public bool shouldSkipMonologue = false;
    #endregion

    #region Events
    public static event EventHandler<OnMonologueEventArgs> OnMonologueBegin;
    public static event EventHandler<OnMonologueEventArgs> OnMonologueEnd;

    public static event EventHandler<OnMonologueEventArgs> OnSentenceBegin;
    public static event EventHandler<OnMonologueEventArgs> OnSentenceEnd;

    public static event EventHandler<OnMonologueEventArgs> OnSentenceIdle;

    public static event EventHandler OnGeneralMonologueBegin;
    public static event EventHandler OnGeneralMonologueConcluded;
    public static event EventHandler OnMidSentences;
    #endregion

    public class OnMonologueEventArgs : EventArgs
    {
        public MonologueSO monologueSO;
        public MonologueSentence monologueSentence;
    }

    private void OnEnable()
    {
        MonologueUI.OnMonologueTransitionInEnd += MonologueUI_OnMonologueTransitionInEnd;
        MonologueUI.OnMonologueTransitionOutEnd += MonologueUI_OnMonologueTransitionOutEnd;
        MonologueUI.OnSentenceTransitionInEnd += MonologueUI_OnSentenceTransitionInEnd;
        MonologueUI.OnSentenceTransitionOutEnd += MonologueUI_OnSentenceTransitionOutEnd;
    }

    private void OnDisable()
    {
        MonologueUI.OnMonologueTransitionInEnd -= MonologueUI_OnMonologueTransitionInEnd;
        MonologueUI.OnMonologueTransitionOutEnd -= MonologueUI_OnMonologueTransitionOutEnd;
        MonologueUI.OnSentenceTransitionInEnd -= MonologueUI_OnSentenceTransitionInEnd;
        MonologueUI.OnSentenceTransitionOutEnd -= MonologueUI_OnSentenceTransitionOutEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetMonologueState(MonologueState.NotOnMonologue);
        ClearCurrentMonologue();
        ClearCurrentSentence();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one MonologueManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Logic
    public void StartMonologue(MonologueSO monologueSO)
    {
        if (!CanStartMonologue()) return;
        if (monologueSO.monologueSentences.Count <= 0) return;

        StartCoroutine(MonologueCoroutine(monologueSO));
    }

    public void EndSentence()
    {
        if (monologueState != MonologueState.Idle) return;
        shouldSkipSentence = true;
    }

    public void EndMonologue()
    {
        if (monologueState != MonologueState.Idle) return;
        shouldSkipMonologue = true;
    }

    private IEnumerator MonologueCoroutine(MonologueSO monologueSO)
    {
        OnGeneralMonologueBegin?.Invoke(this, EventArgs.Empty);

        SetCurrentMonologue(monologueSO);

        for (int i = 0; i < monologueSO.monologueSentences.Count; i++)
        {
            SetCurrentSentence(monologueSO.monologueSentences[i]);

            #region Monologue Begin Logic & Sentence Transition In Logic
            if (i == 0) //If first Sentence, MonologueIsStarting & Wait for the TransitionIn To Complete
            {
                SetMonologueState(MonologueState.MonologueTransitionIn);

                monologueTransitionInCompleted = false;
                OnMonologueBegin?.Invoke(this, new OnMonologueEventArgs { monologueSentence = currentSentence });

                yield return new WaitUntil(() => monologueTransitionInCompleted);//Wait for TransitionInCompleted
                monologueTransitionInCompleted = false;
            }
            else //Otherwise transition to a sentence
            {
                SetMonologueState(MonologueState.SentenceTransitionIn);

                sentenceTransitionInCompleted = false;
                OnSentenceBegin?.Invoke(this, new OnMonologueEventArgs { monologueSentence = currentSentence });

                yield return new WaitUntil(() => sentenceTransitionInCompleted);
                sentenceTransitionInCompleted = false;
            }
            #endregion

            #region Idle Logic
            //At this point, Sentence Is On Idle

            shouldSkipMonologue = false;
            shouldSkipSentence = false;

            SetMonologueState(MonologueState.Idle);

            OnSentenceIdle?.Invoke(this, new OnMonologueEventArgs { monologueSentence = currentSentence });

            #region Wait Sentence Time Logic

            float waitTime = 0;

            while (waitTime <= currentSentence.time)
            {
                if (shouldSkipSentence || shouldSkipMonologue) break;

                waitTime += Time.deltaTime;
                yield return null;
            }
            #endregion

            shouldSkipSentence = false;

            if (shouldSkipMonologue) break;
            #endregion

            #region Transition Sentence Out Logic
            if (i + 1 < monologueSO.monologueSentences.Count) //If it is not the last
            {
                SetMonologueState(MonologueState.SentenceTransitionOut);

                sentenceTransitionOutCompleted = false;
                OnSentenceEnd?.Invoke(this, new OnMonologueEventArgs { monologueSentence = currentSentence });

                yield return new WaitUntil(() => sentenceTransitionOutCompleted);
                sentenceTransitionOutCompleted = false;

                OnMidSentences?.Invoke(this, EventArgs.Empty);
            }
            #endregion
        }

        shouldSkipMonologue = false;

        SetMonologueState(MonologueState.MonologueTransitionOut);

        monologueTransitionOutCompleted = false;
        OnMonologueEnd?.Invoke(this, new OnMonologueEventArgs { monologueSentence = currentSentence });

        yield return new WaitUntil(() => monologueTransitionOutCompleted);
        monologueTransitionOutCompleted = false;

        OnGeneralMonologueConcluded.Invoke(this, EventArgs.Empty);
        SetMonologueState(MonologueState.NotOnMonologue);

        ClearCurrentMonologue();
        ClearCurrentSentence();
    }
    #endregion

    private bool CanStartMonologue()
    {
        if (monologueState != MonologueState.NotOnMonologue) return false;
        return true;
    }

    #region States
    private void SetMonologueState(MonologueState monologueState) => this.monologueState = monologueState;

    #endregion

    #region Setters
    private void SetCurrentMonologue(MonologueSO monologueSO) => currentMonologueSO = monologueSO;
    private void ClearCurrentMonologue() => currentMonologueSO = null;

    private void SetCurrentSentence(MonologueSentence sentence) => currentSentence = sentence;
    private void ClearCurrentSentence() => currentSentence = null;
    #endregion

    #region Subscriptions
    private void MonologueUI_OnMonologueTransitionInEnd(object sender, MonologueUI.OnMonologueSentenceEventArgs e) => monologueTransitionInCompleted = true;
    private void MonologueUI_OnMonologueTransitionOutEnd(object sender, MonologueUI.OnMonologueSentenceEventArgs e) => monologueTransitionOutCompleted = true;
    private void MonologueUI_OnSentenceTransitionInEnd(object sender, MonologueUI.OnMonologueSentenceEventArgs e) => sentenceTransitionInCompleted = true;
    private void MonologueUI_OnSentenceTransitionOutEnd(object sender, MonologueUI.OnMonologueSentenceEventArgs e) => sentenceTransitionOutCompleted = true;
    #endregion
}
