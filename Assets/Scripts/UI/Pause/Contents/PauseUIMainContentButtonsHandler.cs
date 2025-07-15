using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseUIMainContentButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PauseUIContentsHandler pauseUIContentsHandler;

    [Header("InGameOptions Button")]
    [SerializeField] private Button inGameOptionsButton;

    [Header("BackToMenu Button")]
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private string mainMenuScene;
    [SerializeField] private TransitionType menuTransitionType;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        inGameOptionsButton.onClick.AddListener(ShowInGameOptionsContent);
        backToMenuButton.onClick.AddListener(BackToMenu);
    }

    private void ShowInGameOptionsContent() => pauseUIContentsHandler.ShowInGameOptionsContent();

    private void BackToMenu()
    {
        ScenesManager.Instance.TransitionLoadTargetScene(mainMenuScene, menuTransitionType);
    }
}
