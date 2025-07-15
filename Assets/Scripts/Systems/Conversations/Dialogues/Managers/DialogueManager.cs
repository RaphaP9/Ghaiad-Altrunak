using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private DialogueSO currentDialogueSO;
    [SerializeField] private DialogueSentence currentSentence;

    [Header("States - Runtime Filled")]
    [SerializeField] private DialogueState dialogueState;

    public DialogueState State => dialogueState;

    public enum DialogueState { NotOnDialogue, DialogueTransitionIn, DialogueTransitionOut, Idle, SentenceTransitionIn, SentenceTransitionOut }

    #region Flags
    private bool dialogueTransitionInCompleted = false;
    private bool dialogueTransitionOutCompleted = false;

    private bool sentenceTransitionInCompleted = false;
    private bool sentenceTransitionOutCompleted = false;

    private bool shouldSkipSentence = false;
    private bool shouldSkipDialogue = false;
    #endregion

    #region Events
    public static event EventHandler<OnDialogueEventArgs> OnDialogueBegin;   
    public static event EventHandler<OnDialogueEventArgs> OnDialogueEnd;

    public static event EventHandler<OnDialogueEventArgs> OnSentenceBegin;
    public static event EventHandler<OnDialogueEventArgs> OnSentenceEnd;

    public static event EventHandler<OnDialogueEventArgs> OnSentenceIdle;

    public static event EventHandler OnGeneralDialogueBegin;
    public static event EventHandler OnGeneralDialogueConcluded;
    public static event EventHandler OnMidSentences;
    #endregion

    public class OnDialogueEventArgs : EventArgs
    {
        public DialogueSO dialogueSO;
        public DialogueSentence dialogueSentence;
    }

    private void OnEnable()
    {
        DialogueUI.OnDialogueTransitionInEnd += DialogueUI_OnDialogueTransitionInEnd;
        DialogueUI.OnDialogueTransitionOutEnd += DialogueUI_OnDialogueTransitionOutEnd;
        DialogueUI.OnSentenceTransitionInEnd += DialogueUI_OnSentenceTransitionInEnd;
        DialogueUI.OnSentenceTransitionOutEnd += DialogueUI_OnSentenceTransitionOutEnd;

        TypewriterHandler.OnCompleteSentenceCommand += TypewriterHandler_OnCompleteSentenceCommand;
    }

    private void OnDisable()
    {
        DialogueUI.OnDialogueTransitionInEnd -= DialogueUI_OnDialogueTransitionInEnd;
        DialogueUI.OnDialogueTransitionOutEnd -= DialogueUI_OnDialogueTransitionOutEnd;
        DialogueUI.OnSentenceTransitionInEnd -= DialogueUI_OnSentenceTransitionInEnd;
        DialogueUI.OnSentenceTransitionOutEnd -= DialogueUI_OnSentenceTransitionOutEnd;

        TypewriterHandler.OnCompleteSentenceCommand -= TypewriterHandler_OnCompleteSentenceCommand;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetDialogueState(DialogueState.NotOnDialogue);
        ClearCurrentDialogue();
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
            Debug.LogWarning("There is more than one DialogueManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Logic
    public void StartDialogue(DialogueSO dialogueSO)
    {
        if (!CanStartDialogue()) return;
        if (dialogueSO.dialogueSentences.Count <= 0) return;

        StartCoroutine(DialogueCoroutine(dialogueSO));
    }

    public void EndSentence()
    {
        if (dialogueState != DialogueState.Idle) return;
        shouldSkipSentence = true;
    }

    public void EndDialogue()
    {
        if (dialogueState != DialogueState.Idle) return;
        shouldSkipDialogue = true;
    }

    private IEnumerator DialogueCoroutine(DialogueSO dialogueSO)
    {
        OnGeneralDialogueBegin?.Invoke(this, EventArgs.Empty);

        SetCurrentDialogue(dialogueSO);

        for(int i=0; i< dialogueSO.dialogueSentences.Count; i++)
        {
            SetCurrentSentence(dialogueSO.dialogueSentences[i]);

            #region Dialogue Begin Logic & Sentence Transition In Logic
            if (i ==0) //If first Sentence, DialogueIsStarting & Wait for the TransitionIn To Complete
            {
                SetDialogueState(DialogueState.DialogueTransitionIn);

                dialogueTransitionInCompleted = false;
                OnDialogueBegin?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence });

                yield return new WaitUntil(() => dialogueTransitionInCompleted);//Wait for TransitionInCompleted
                dialogueTransitionInCompleted = false;
            }
            else if(dialogueSO.dialogueSentences[i].triggerSentenceTransition) //If current sentence has the triggerSentenceTransition Checked
            {
                SetDialogueState(DialogueState.SentenceTransitionIn);

                sentenceTransitionInCompleted = false;
                OnSentenceBegin?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence });

                yield return new WaitUntil(() => sentenceTransitionInCompleted);
                sentenceTransitionInCompleted = false;
            }
            #endregion

            #region Idle Logic
            //At this point, Sentence Is On Idle

            shouldSkipDialogue = false;
            shouldSkipSentence = false;

            SetDialogueState(DialogueState.Idle);

            OnSentenceIdle?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence}); //Loads the entire Sentence

            yield return new WaitUntil(() => shouldSkipSentence || shouldSkipDialogue);

            shouldSkipSentence = false;

            if (shouldSkipDialogue) break;
            #endregion

            #region Transition Sentence Out Logic
            if (i + 1 < dialogueSO.dialogueSentences.Count) //If it is not the last
            {
                if (dialogueSO.dialogueSentences[i + 1].triggerSentenceTransition) //If next sentence has the triggerSentenceTransition Checked
                {
                    SetDialogueState(DialogueState.SentenceTransitionOut);

                    sentenceTransitionOutCompleted = false;
                    OnSentenceEnd?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence });

                    yield return new WaitUntil(() => sentenceTransitionOutCompleted);
                    sentenceTransitionOutCompleted = false;
                }

                OnMidSentences?.Invoke(this, EventArgs.Empty);
            }
            #endregion
        }

        shouldSkipDialogue = false;

        SetDialogueState(DialogueState.DialogueTransitionOut);

        dialogueTransitionOutCompleted = false;
        OnDialogueEnd?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence });

        yield return new WaitUntil(() => dialogueTransitionOutCompleted);
        dialogueTransitionOutCompleted = false;

        OnGeneralDialogueConcluded.Invoke(this, EventArgs.Empty);
        SetDialogueState(DialogueState.NotOnDialogue);

        ClearCurrentDialogue();
        ClearCurrentSentence();
    }
    #endregion

    private bool CanStartDialogue()
    {
        if (dialogueState != DialogueState.NotOnDialogue) return false;
        return true;
    }

    #region States
    private void SetDialogueState(DialogueState dialogueState) => this.dialogueState = dialogueState;

    #endregion

    #region Setters
    private void SetCurrentDialogue(DialogueSO dialogueSO) => currentDialogueSO = dialogueSO;
    private void ClearCurrentDialogue() => currentDialogueSO = null;   

    private void SetCurrentSentence(DialogueSentence sentence) => currentSentence = sentence;
    private void ClearCurrentSentence() => currentSentence = null;
    #endregion

    #region Subscriptions
    private void DialogueUI_OnDialogueTransitionInEnd(object sender, DialogueUI.OnDialogueSentenceEventArgs e) => dialogueTransitionInCompleted = true;
    private void DialogueUI_OnDialogueTransitionOutEnd(object sender, DialogueUI.OnDialogueSentenceEventArgs e) => dialogueTransitionOutCompleted = true;
    private void DialogueUI_OnSentenceTransitionInEnd(object sender, DialogueUI.OnDialogueSentenceEventArgs e) => sentenceTransitionInCompleted = true;
    private void DialogueUI_OnSentenceTransitionOutEnd(object sender, DialogueUI.OnDialogueSentenceEventArgs e) => sentenceTransitionOutCompleted = true;

    private void TypewriterHandler_OnCompleteSentenceCommand(object sender, EventArgs e)
    {
        EndSentence();
    }
    #endregion
}
