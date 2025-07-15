using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIInGameAnyOptionsContentButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PauseUIContentsHandler pauseUIContentsHandler;

    [Header("UI Components")]
    [SerializeField] private Button backButton;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        backButton.onClick.AddListener(ShowInGameOptionsContent);
    }

    private void ShowInGameOptionsContent() => pauseUIContentsHandler.ShowInGameOptionsContent();
}
