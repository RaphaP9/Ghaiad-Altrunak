using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIAnyOptionsContentsButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private OptionsUIContentsHandler optionsUIContentsHandler;

    [Header("UI Components")]
    [SerializeField] private Button backButton;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        backButton.onClick.AddListener(ShowMainContent);
    }

    private void ShowMainContent() => optionsUIContentsHandler.ShowMainContent();
}
