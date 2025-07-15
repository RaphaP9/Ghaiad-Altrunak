using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(2f, 5f)] private float timeToEndAfterWin;
    [Space]
    [SerializeField] private bool wipeRunDataOnWin;

    public static event EventHandler<OnRunCompletedEventArgs> OnDataUpdateOnRunCompleted;

    public class OnRunCompletedEventArgs : EventArgs
    {
        public CharacterSO characterSO;
    }

    private void OnEnable()
    {
        GameManager.OnGameWon += GameManager_OnGameWon;
    }

    private void OnDisable()
    {
        GameManager.OnGameWon -= GameManager_OnGameWon;
    }

    private void WinGame()
    {
        OnDataUpdateOnRunCompleted?.Invoke(this, new OnRunCompletedEventArgs { characterSO = PlayerCharacterManager.Instance.CharacterSO });

        if (wipeRunDataOnWin)
        {
            DataUtilities.WipeRunData();
            RunDataContainer.Instance.ResetRunData();
        }

        StartCoroutine(WinGameCoroutine());
    }

    private IEnumerator WinGameCoroutine()
    {
        yield return new WaitForSeconds(timeToEndAfterWin);
        GeneralSceneSettings.Instance.TransitionToWinScene();
    }

    private void GameManager_OnGameWon(object sender, System.EventArgs e)
    {
        WinGame();
    }
}

