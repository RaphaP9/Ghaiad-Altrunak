using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI sentenceText;
    [Space]
    [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [Space]
    [SerializeField] private Image dialogueBoxImage;
    [SerializeField] private Image speakerBoxImage;

    [Header("Positions & Transforms")]
    [SerializeField] private RectTransform speakerGroupTransform;
    [SerializeField] private RectTransform sentenceTextTransform;
    [Space]
    [SerializeField] private RectTransform leftSpeakerGroupPosition;
    [SerializeField] private RectTransform leftSentenceTextPosition;
    [Space]
    [SerializeField] private RectTransform rightSpeakerGroupPosition;
    [SerializeField] private RectTransform rightSentenceTextPosition;

    [Header("Runtime Filled")]
    [SerializeField] private DialogueSentence currentDialogueSentence;

    #region Animation Names
    private const string HIDDEN_ANIMATION_NAME = "Hidden";
    private const string IDLE_ANIMATION_NAME = "Idle";

    private const string DIALOGUE_TRANSITION_IN_ANIMATION_NAME = "DialogueTransitionIn";
    private const string DIALOGUE_TRANSITION_OUT_ANIMATION_NAME = "DialogueTransitionOut";

    private const string SENTENCE_TRANSITION_IN_ANIMATION_NAME = "SentenceTransitionIn";
    private const string SENTENCE_TRANSITION_OUT_ANIMATION_NAME = "SentenceTransitionOut";
    #endregion

    #region Events
    public static event EventHandler<OnDialogueSentenceEventArgs> OnDialogueTransitionInStart;
    public static event EventHandler<OnDialogueSentenceEventArgs> OnDialogueTransitionInEnd;

    public static event EventHandler<OnDialogueSentenceEventArgs> OnSentenceTransitionInStart;
    public static event EventHandler<OnDialogueSentenceEventArgs> OnSentenceTransitionInEnd;

    public static event EventHandler<OnDialogueSentenceEventArgs> OnSentenceTransitionOutStart;
    public static event EventHandler<OnDialogueSentenceEventArgs> OnSentenceTransitionOutEnd;

    public static event EventHandler<OnDialogueSentenceEventArgs> OnDialogueTransitionOutStart;
    public static event EventHandler<OnDialogueSentenceEventArgs> OnDialogueTransitionOutEnd;
    #endregion

    public class OnDialogueSentenceEventArgs : EventArgs
    {
        public DialogueSentence dialogueSentence;
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueBegin += DialogueManager_OnDialogueBegin;
        DialogueManager.OnDialogueEnd += DialogueManager_OnDialogueEnd;
        DialogueManager.OnSentenceBegin += DialogueManager_OnSentenceBegin;
        DialogueManager.OnSentenceEnd += DialogueManager_OnSentenceEnd;
        DialogueManager.OnSentenceIdle += DialogueManager_OnSentenceIdle;
        DialogueManager.OnGeneralDialogueConcluded += DialogueManager_OnGeneralDialogueConcluded;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueBegin -= DialogueManager_OnDialogueBegin;
        DialogueManager.OnDialogueEnd -= DialogueManager_OnDialogueEnd;
        DialogueManager.OnSentenceBegin -= DialogueManager_OnSentenceBegin;
        DialogueManager.OnSentenceEnd -= DialogueManager_OnSentenceEnd;
        DialogueManager.OnSentenceIdle -= DialogueManager_OnSentenceIdle;
        DialogueManager.OnGeneralDialogueConcluded -= DialogueManager_OnGeneralDialogueConcluded;
    }

    private void PlayAnimation(string animationName) => animator.Play(animationName);


    #region Animations
    private void DialogueTransitionIn()
    {
        PlayAnimation(DIALOGUE_TRANSITION_IN_ANIMATION_NAME);
        OnDialogueTransitionInStart?.Invoke(this, new OnDialogueSentenceEventArgs());
    }

    private void DialogueTransitionOut()
    {
        PlayAnimation(DIALOGUE_TRANSITION_OUT_ANIMATION_NAME);
        OnDialogueTransitionOutStart?.Invoke(this, new OnDialogueSentenceEventArgs());
    }

    private void SentenceTransitionIn()
    {
        PlayAnimation(SENTENCE_TRANSITION_IN_ANIMATION_NAME);
        OnSentenceTransitionInStart?.Invoke(this, new OnDialogueSentenceEventArgs());
    }

    private void SentenceTransitionOut()
    {
        PlayAnimation(SENTENCE_TRANSITION_OUT_ANIMATION_NAME);
        OnSentenceTransitionOutStart?.Invoke(this, new OnDialogueSentenceEventArgs());
    }

    private void SentenceIdle()
    {
        PlayAnimation(IDLE_ANIMATION_NAME);
    }

    private void DialogueConcluded()
    {
        PlayAnimation(HIDDEN_ANIMATION_NAME);
    }
    #endregion

    #region Animation Event Methods
    public void TriggerDialogueTransitionInEnd() => OnDialogueTransitionInEnd?.Invoke(this, new OnDialogueSentenceEventArgs { });
    public void TriggerDialogueTransitionOutEnd() => OnDialogueTransitionOutEnd?.Invoke(this, new OnDialogueSentenceEventArgs { });
    public void TriggerSentenceTransitionInEnd() => OnSentenceTransitionInEnd?.Invoke(this, new OnDialogueSentenceEventArgs { });
    public void TriggerSentenceTransitionOutEnd() => OnSentenceTransitionOutEnd?.Invoke(this, new OnDialogueSentenceEventArgs { });
    #endregion

    #region Set 
    private void SetSentenceUI(DialogueSentence dialogueSentence)
    {
        SetCurrentDialogueSentence(dialogueSentence);

        //sentenceText.text = dialogueSentence.sentenceText; //Handled By TypewritterHandler
        speakerImage.sprite = dialogueSentence.dialogueSpeakerSO.speakerImage;
        speakerNameText.text = dialogueSentence.dialogueSpeakerSO.speakerName;
        speakerNameText.color = dialogueSentence.dialogueSpeakerSO.nameColor;

        dialogueBoxImage.sprite = dialogueSentence.dialogueBoxSprite;
        speakerBoxImage.sprite = dialogueSentence.speakerBoxSprite;

        if (dialogueSentence.speakerOnRight) SetRightSpeakerUIPosition();
        else SetLeftSpeakerUIPosition();
    }

    private void SetLeftSpeakerUIPosition()
    {
        speakerGroupTransform.position = leftSpeakerGroupPosition.position;
        sentenceTextTransform.position = leftSentenceTextPosition.position;
    }

    private void SetRightSpeakerUIPosition()
    {
        speakerGroupTransform.position = rightSpeakerGroupPosition.position;
        sentenceTextTransform.position = rightSentenceTextPosition.position;
    }

    private void SetCurrentDialogueSentence(DialogueSentence dialogueSentence) => currentDialogueSentence = dialogueSentence;
    #endregion

    #region Subscriptions
    private void DialogueManager_OnDialogueBegin(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        SetSentenceUI(e.dialogueSentence);
        DialogueTransitionIn();
    }

    private void DialogueManager_OnDialogueEnd(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        DialogueTransitionOut();
    }

    private void DialogueManager_OnSentenceBegin(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        SetSentenceUI(e.dialogueSentence);
        SentenceTransitionIn();
    }

    private void DialogueManager_OnSentenceEnd(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        SentenceTransitionOut();
    }

    private void DialogueManager_OnSentenceIdle(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        SetSentenceUI(e.dialogueSentence);
        SentenceIdle();
    }

    private void DialogueManager_OnGeneralDialogueConcluded(object sender, EventArgs e)
    {
        DialogueConcluded();
    }
    #endregion
}
