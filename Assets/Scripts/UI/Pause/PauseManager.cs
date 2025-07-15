using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    public static event EventHandler OnGamePaused;
    public static event EventHandler OnGameResumed;

    private bool PauseInput => UIInput.GetPauseDown();
    public bool GamePaused { get; private set; }
    public bool GamePausedThisFrame { get; private set; }
    //Used to check if UIManager should ignore the CheckClose that frame, because both Pause and CheckClose use the GetPauseDown() input
    //It could happen that when the game is paused, PauseUI is opened and inmediately closed

    private void OnEnable()
    {
        PauseUI.OnCloseFromUI += PauseUI_OnCloseFromUI;
    }

    private void OnDisable()
    {
        PauseUI.OnCloseFromUI -= PauseUI_OnCloseFromUI;
        SetResumeTimeScale();
        SetGamePaused(false);
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckPauseResumeGame();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PauseManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    private void InitializeVariables()
    {
        SetGamePaused(false);
        GamePausedThisFrame = false;
        AudioListener.pause = false;
    }

    private void CheckPauseResumeGame()
    {
        GamePausedThisFrame = false;

        if (!PauseInput) return;

        if (!GamePaused)
        {
            if (UILayersManager.Instance.UILayerActive) return;

            PauseGame();
            GamePausedThisFrame = true;

            UIInput.SetInputOnCooldown();
        }
        else
        {
            if (UILayersManager.Instance.GetUILayersCount() == 1) //If count is 1, the active layer is the PauseUI
            {
                ResumeGame();
            }

            UIInput.SetInputOnCooldown();
        }
    }

    private void PauseGame()
    {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
        SetPauseTimeScale();
        SetGamePaused(true);
        AudioListener.pause = false;
    }

    private void ResumeGame()
    {
        OnGameResumed?.Invoke(this, EventArgs.Empty);
        SetResumeTimeScale();
        SetGamePaused(false);
        AudioListener.pause = false;
    }

    private void SetGamePaused(bool gamePaused) => GamePaused = gamePaused;
    private void SetPauseTimeScale() => Time.timeScale = 0f;
    private void SetResumeTimeScale() => Time.timeScale = 1f;

    #region PauseUI Subscriptions
    private void PauseUI_OnCloseFromUI(object sender, EventArgs e)
    {
        ResumeGame();
    }
    #endregion

}
