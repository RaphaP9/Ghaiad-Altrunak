using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StagePresentationUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI stageNameText;

    [Header("Settings")]
    [SerializeField, Range (0f, 7f)] private float timeToShowStageName;
    [SerializeField, Range (1f,7f)] private float timeShowingStageName;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        GeneralStagesManager.OnStageInitialized += GeneralStagesManager_OnStageInitialized;
        GeneralStagesManager.OnStageChange += GeneralStagesManager_OnStageChange;
    }

    private void OnDisable()
    {
        GeneralStagesManager.OnStageInitialized -= GeneralStagesManager_OnStageInitialized;
        GeneralStagesManager.OnStageChange -= GeneralStagesManager_OnStageChange;
    }

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

    private void SetStageNameText(string stageName) => stageNameText.text = stageName;  
    private void SetStageNameColor(Color stageNameColor) => stageNameText.color = stageNameColor;

    private IEnumerator ShowStageNameCoroutine(StageSO stageSO)
    {
        SetStageNameText(stageSO.stageName);
        SetStageNameColor(stageSO.stageNameColor);

        yield return new WaitForSeconds(timeToShowStageName);

        ShowUI();

        yield return new WaitForSeconds(timeShowingStageName);

        HideUI();
    }

    private void GeneralStagesManager_OnStageInitialized(object sender, GeneralStagesManager.OnStageChangeEventArgs e)
    {
        StartCoroutine(ShowStageNameCoroutine(e.stageGroup.stageSO));   
    }

    private void GeneralStagesManager_OnStageChange(object sender, GeneralStagesManager.OnStageChangeEventArgs e)
    {
        StartCoroutine(ShowStageNameCoroutine(e.stageGroup.stageSO));
    }
}
