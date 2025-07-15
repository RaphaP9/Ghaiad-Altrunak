using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIMainContentButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private OptionsUIContentsHandler optionsUIContentsHandler;

    [Header("UI Components")]
    [SerializeField] private Button audioOptionsButton;
    [SerializeField] private Button graphicsOptionsButton;
    [SerializeField] private Button controlsOptionsButton;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        audioOptionsButton.onClick.AddListener(ShowAudioContent);
        graphicsOptionsButton.onClick.AddListener(ShowGraphicsContent);
        controlsOptionsButton.onClick.AddListener(ShowControlsContent);
    }

    private void ShowAudioContent() => optionsUIContentsHandler.ShowAudioOptionsContent();
    private void ShowGraphicsContent() => optionsUIContentsHandler.ShowGraphicsOptionsContent();
    private void ShowControlsContent() => optionsUIContentsHandler.ShowControlsOptionsContent();
}
