using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChangeUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
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

    private void GameManager_OnStateChanged(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if(e.newState == GameManager.State.BeginningChangingStage)
        {
            ShowUI();
            return;
        }

        if (e.newState == GameManager.State.EndingChangingStage)
        {
            HideUI();
            return;
        }
    }
}
