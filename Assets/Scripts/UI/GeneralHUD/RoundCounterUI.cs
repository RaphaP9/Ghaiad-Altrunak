using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundCounterUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UIComponents")]
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI roundText;

    private int bufferedStageNumber = 0;
    private int bufferedRoundNumber = 0;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string SHOWING_ANIMATION_NAME = "Showing";
    private const string HIDDEN_ANIMATION_NAME = "Hidden";


    #region Show&Hide
    private void ShowUI()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideUI()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void ShowUIImmediately()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.ResetTrigger(HIDE_TRIGGER);

        animator.Play(SHOWING_ANIMATION_NAME);
    }
    private void HideUIImmediately()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.ResetTrigger(HIDE_TRIGGER);

        animator.Play(HIDDEN_ANIMATION_NAME);
    }
    #endregion

    private void UpdateUIToBufferedValues()
    {
        stageText.text = bufferedStageNumber.ToString();
        roundText.text = bufferedRoundNumber.ToString();
    }

    #region Subscriptions
    private void GeneralStagesManager_OnStageAndRoundInitialized(object sender, GeneralStagesManager.OnStageAndRoundEventArgs e)
    {
        bufferedStageNumber = e.stageNumber;
        bufferedRoundNumber = e.roundNumber;
    }
    private void GeneralStagesManager_OnStageAndRoundLoad(object sender, GeneralStagesManager.OnStageAndRoundLoadEventArgs e)
    {
        bufferedStageNumber = e.newStageNumber;
        bufferedRoundNumber = e.newRoundNumber;
    }
    #endregion
}
