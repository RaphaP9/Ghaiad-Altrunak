using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogManager : MonoBehaviour
{
    public static GameLogManager Instance { get; private set; }

    [Header("Log")]
    [SerializeField] private List<GameplayAction> gameLog;
    [SerializeField] private bool enableGameLog;

    public List<GameplayAction> GameLog => gameLog;

    [Serializable]
    public struct GameplayAction
    {
        public float time;
        public string log;
    }

    public static event EventHandler<OnLogAddEventArgs> OnLogAdd;

    public class OnLogAddEventArgs : EventArgs
    {
        public GameplayAction gameplayAction;
    }

    private void Awake()
    {
        SetSingleton();
        InitializeLog();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameLog instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeLog()
    {
        gameLog = new List<GameplayAction>();
    }

    public void Log(string log)
    {
        if (!enableGameLog) return;

        GameplayAction gameplayAction = new GameplayAction { time = Time.time, log = log };

        Instance.gameLog.Add(gameplayAction);

        OnLogAdd?.Invoke(this, new OnLogAddEventArgs { gameplayAction = gameplayAction });
    }
}
