using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUIButtonsHandler : MonoBehaviour
{
    [Header("Back To Menu Button")]
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private string menuScene;
    [SerializeField] private TransitionType menuTransitionType;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        backToMenuButton.onClick.AddListener(BackToMenu);
    }

    private void BackToMenu()
    {
        ScenesManager.Instance.TransitionLoadTargetScene(menuScene, menuTransitionType);
    }
}
