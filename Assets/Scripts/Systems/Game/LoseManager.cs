using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoseManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(2f, 5f)] private float timeToEndAfterLose;
    [Space]
    [SerializeField] private bool wipeRunDataOnLose;

    public static event EventHandler<OnRunLostEventArgs> OnDataUpdateOnRunLost;

    public class OnRunLostEventArgs : EventArgs
    {
        public CharacterSO characterSO;
    }

    private void OnEnable()
    {
        GameManager.OnGameLost += GameManager_OnGameLost;
    }

    private void OnDisable()
    {
        GameManager.OnGameLost -= GameManager_OnGameLost;
    }

    private void LoseGame()
    {
        OnDataUpdateOnRunLost?.Invoke(this, new OnRunLostEventArgs { characterSO = PlayerCharacterManager.Instance.CharacterSO });

        if (wipeRunDataOnLose)
        {
            DataUtilities.WipeRunData();
            RunDataContainer.Instance.ResetRunData();
        }

        StartCoroutine(LoseGameCoroutine());
    }

    private IEnumerator LoseGameCoroutine()
    {
        yield return new WaitForSeconds(timeToEndAfterLose);
        GeneralSceneSettings.Instance.TransitionToLoseScene();
    }

    private void GameManager_OnGameLost(object sender, System.EventArgs e)
    {
        LoseGame();
    }

}
