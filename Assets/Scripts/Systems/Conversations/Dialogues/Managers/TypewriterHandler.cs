using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI sentenceText;

    [Header("Typewriter Settings")]
    [SerializeField, Range(0f, 2f)] private float timeToStartTypewriting;
    [SerializeField, Range(1, 100)] private int charactersPerSecond;
    [SerializeField, Range(0f, 3f)] private float interpuntuationDelay;
    [Space]
    [SerializeField] private bool speedUpSkip;
    [SerializeField, Range(2, 50)] private int skipSpeedMultiplier;

    [Header("Runtime Filled")]
    [SerializeField] private string typewriterText;
    private bool SkipInput => ConversationsInput.Instance.GetSkipDown();

    private const char PERIOD_CHARACTER = '.';
    private const char COMA_CHARACTER = ',';
    private const char CLOSING_QUESTION_CHARACTER = '?';
    private const char CLOSING_EXCLAMATION_CHARACTER = '!';

    //Runtime Filled Values
    private int currentVisibleCharacterIndex;
    private float regularDelay;
    private float skipDelay;

    //Coroutines
    private Coroutine typewriterCoroutine;
    private Coroutine delayCoroutine;
    private Coroutine preciseDelayCoroutine;

    //Flags
    private bool delayCompleted = false;
    private bool shouldSkip = false;
    private bool typingCompleted = false;

    #region Events
    //public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceStart;
    //public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceTypingBegin;
    //public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceTypingSkip;
    //public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceTypingComplete;
    //public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceEnd;
    //public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterCompleted;

    public static event EventHandler OnCompleteSentenceCommand;
    #endregion

    #region Classes
    public class OnTypewriterSentenceEventArgs : EventArgs
    {
        public string typewriterText;
    }
    #endregion

    private void OnEnable()
    {
        DialogueManager.OnSentenceIdle += DialogueManager_OnSentenceIdle;

        DialogueManager.OnGeneralDialogueBegin += DialogueManager_OnGeneralDialogueBegin;
        DialogueManager.OnGeneralDialogueConcluded += DialogueManager_OnGeneralDialogueConcluded;
        DialogueManager.OnMidSentences += DialogueManager_OnMidSentences;
    }

    private void OnDisable()
    {
        DialogueManager.OnSentenceIdle -= DialogueManager_OnSentenceIdle;

        DialogueManager.OnGeneralDialogueBegin -= DialogueManager_OnGeneralDialogueBegin;
        DialogueManager.OnGeneralDialogueConcluded -= DialogueManager_OnGeneralDialogueConcluded;
        DialogueManager.OnMidSentences -= DialogueManager_OnMidSentences;
    }

    private void Start()
    {
        ResetFlags();
        InitializeVariables();
        CompleteTypewriterReset();
    }

    private void Update()
    {
        HandleSkip();
    }

    private void InitializeVariables()
    {
        regularDelay = 1f / charactersPerSecond;
        skipDelay = regularDelay / skipSpeedMultiplier;
    }

    private void ResetFlags()
    {
        shouldSkip = false;
        delayCompleted = false;
        typingCompleted = false;
    }

    private void HandleSkip()
    {
        if (!SkipInput) return;
        if (typewriterCoroutine == null) return; //No Typewritting Happening

        if (!typingCompleted) shouldSkip = true;
        else OnCompleteSentenceCommand?.Invoke(this, EventArgs.Empty);
    }

    #region Logic
    private void StartTypewriting(string text)
    {
        if (typewriterCoroutine != null) StopCoroutine(typewriterCoroutine);

        sentenceText.text = text;

        AssignTypewritterCoroutineRefference(text); //Includes Coroutine Start
    }

    private IEnumerator TypewriterCoroutine(string textToTypewrite)
    {
        typingCompleted = false;
        shouldSkip = false;

        UpdateTypewriterText(textToTypewrite);

        ResetMaxVisibleCharacters();
        ResetCurrentVisibleCharacterIndex();

        sentenceText.ForceMeshUpdate();

        TMP_TextInfo textInfo = sentenceText.textInfo;
        int lastCharacterIndex = textInfo.characterCount - 1;

        #region TimeToStartTypewritingLogic Logic
        delayCompleted = false;
        AssignDelayCoroutineRefference(timeToStartTypewriting);
        yield return new WaitUntil(() => shouldSkip || delayCompleted);
        ResetDelayCoroutineRefference();
        delayCompleted = false;
        #endregion

        while (currentVisibleCharacterIndex < textInfo.characterCount)
        {
            if (shouldSkip && !speedUpSkip) break;

            sentenceText.maxVisibleCharacters++;
            sentenceText.ForceMeshUpdate();

            char character = textInfo.characterInfo[currentVisibleCharacterIndex].character;

            float delayBetweenCharacters = EvaluateInterpuntuationCharacter(character) && currentVisibleCharacterIndex != lastCharacterIndex ? interpuntuationDelay : regularDelay;
            delayBetweenCharacters = (shouldSkip && speedUpSkip)? skipDelay : delayBetweenCharacters;

            #region DelayBetweenCharacters Logic
            delayCompleted = false;
            AssignPreciseDelayCoroutineRefference(delayBetweenCharacters);
            yield return new WaitUntil(() => delayCompleted || (shouldSkip && EvaluateInterpuntuationCharacter(character)));
            ResetPreciseDelayCoroutineRefference();
            delayCompleted = false;
            #endregion

            currentVisibleCharacterIndex++;
        }


        sentenceText.maxVisibleCharacters = textInfo.characterCount;
        sentenceText.ForceMeshUpdate();

        shouldSkip = false;
        typingCompleted = true;
    }

    private IEnumerator DelayCoroutine(float delay)
    {
        delayCompleted = false;
        yield return new WaitForSeconds(delay);
        delayCompleted = true;
    }

    private IEnumerator PreciseDelayCoroutine(float delay) //Use only for small delay values
    {
        delayCompleted = false;

        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < delay)
        {          
            while (PauseManager.Instance.GamePaused) yield return null; //While GameIsPaused Delay Should not end (Optional as the delay is very small, this might be inperceptible)
                                                                        
            yield return null;
        }

        delayCompleted = true;
    }

    #endregion

    #region Evaluators
    private bool EvaluateInterpuntuationCharacter(char character)
    {
        if (character == PERIOD_CHARACTER) return true;
        if (character == COMA_CHARACTER) return true;
        if (character == CLOSING_QUESTION_CHARACTER) return true;
        if (character == CLOSING_EXCLAMATION_CHARACTER) return true;

        return false;
    }
    #endregion

    #region Setters
    private void ResetCurrentVisibleCharacterIndex() => currentVisibleCharacterIndex = 0;
    private void ResetMaxVisibleCharacters() => sentenceText.maxVisibleCharacters = 0;

    private void AssignTypewritterCoroutineRefference(string textToTypewrite) => typewriterCoroutine = StartCoroutine(TypewriterCoroutine(textToTypewrite));
    private void ResetTypewritterCoroutineRefference() => typewriterCoroutine = null;

    private void AssignDelayCoroutineRefference(float delay) => delayCoroutine = StartCoroutine(DelayCoroutine(delay));
    private void ResetDelayCoroutineRefference() => delayCoroutine = null;

    private void AssignPreciseDelayCoroutineRefference(float delay) => preciseDelayCoroutine = StartCoroutine(PreciseDelayCoroutine(delay));
    private void ResetPreciseDelayCoroutineRefference() => preciseDelayCoroutine = null;


    private void UpdateTypewriterText(string text)
    {
        typewriterText = text;
        sentenceText.text = text;
    }

    private void ClearTypewriterText()
    {
        typewriterText = "";
        sentenceText.text = "";
    }

    private void CompleteTypewriterReset()
    {
        ResetTypewritterCoroutineRefference();
        ResetDelayCoroutineRefference();
        ResetPreciseDelayCoroutineRefference();

        ResetCurrentVisibleCharacterIndex();
        ResetMaxVisibleCharacters();

        ClearTypewriterText();
    }
    #endregion

    #region Subscriptions

    private void DialogueManager_OnSentenceIdle(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        StartTypewriting(e.dialogueSentence.sentenceText);
    }

    private void DialogueManager_OnGeneralDialogueBegin(object sender, EventArgs e)
    {
        CompleteTypewriterReset();
    }

    private void DialogueManager_OnGeneralDialogueConcluded(object sender, EventArgs e)
    {
        CompleteTypewriterReset();
    }

    private void DialogueManager_OnMidSentences(object sender, EventArgs e)
    {
        CompleteTypewriterReset();
    }
    #endregion
}

