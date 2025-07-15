using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIButtonsHandler : MonoBehaviour
{
    [Header("NewGame")]
    [SerializeField] private Button newGameButton;

    [Header("Continue")]
    [SerializeField] private Button continueButton;
    [SerializeField] private TransitionType continueTransitionType;
    [SerializeField] private string continueScene;

    [Header("Credits")]
    [SerializeField] private Button creditsButton;
    [SerializeField] private TransitionType creditsTransitionType;
    [SerializeField] private string creditsScene;

    [Header("Other")]
    [SerializeField] private Button quitButton;
    [SerializeField] private Button deleteDataButton;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        CheckContiueButtonAvailable();
    }

    private void InitializeButtonsListeners()
    {
        newGameButton.onClick.AddListener(StartNewGame);
        continueButton.onClick.AddListener(LoadContinueGameScene);
        creditsButton.onClick.AddListener(LoadCreditsScene);
        quitButton.onClick.AddListener(QuitGame);
        deleteDataButton.onClick.AddListener(DeleteData);
    }

    private void CheckContiueButtonAvailable()
    {
        if (!DataUtilities.HasSavedRunData()) SetContinueButton(false);
        else SetContinueButton(true);
    }

    private void SetContinueButton(bool enable)
    {
        if (enable) continueButton.gameObject.SetActive(true);
        else continueButton.gameObject.SetActive(false);
    }

    private void LoadContinueGameScene() => ScenesManager.Instance.TransitionLoadTargetScene(continueScene, continueTransitionType); //Do not Delete Any Data
    private void LoadCreditsScene() => ScenesManager.Instance.TransitionLoadTargetScene(creditsScene, creditsTransitionType);
    private void StartNewGame()
    {
        DataUtilities.WipeRunData(); //Delete JSON Run Data
        SessionRunDataContainer.Instance.ResetRunData(); //Reset the Run Data in Data Container

        GeneralSceneSettings.Instance.TransitionToNewGameScene();
    }

    private void DeleteData()
    {
        SessionRunDataContainer.Instance.ResetRunData(); //Reset the Run Data in Data Container
        SessionPerpetualDataContainer.Instance.ResetPerpetualData(); //Reset the Perpetual Data in Data Container

        DataUtilities.WipeAllData();

        CheckContiueButtonAvailable();
    }
    private void QuitGame() => ScenesManager.Instance.QuitGame();
}
