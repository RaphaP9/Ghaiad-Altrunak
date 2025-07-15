using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CentralizedInputSystemManager : MonoBehaviour
{
    public static CentralizedInputSystemManager Instance { get; private set; }
    public PlayerInputActions PlayerInputActions { get; private set; }

    public static EventHandler<OnPlayerInputActionsEventArgs> OnPlayerInputActionsInitialized;
    public static event EventHandler<OnRebindingEventArgs> OnRebindingStarted;
    public static event EventHandler<OnRebindingEventArgs> OnRebindingCompleted;

    private Dictionary<Binding, BindingData> bindingsDictionary;

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public class OnPlayerInputActionsEventArgs : EventArgs
    {
        public PlayerInputActions playerInputActions;
    }

    public class OnRebindingEventArgs : EventArgs
    {
        public Binding binding;
    }

    private void Awake()
    {
        SetSingleton();
        InitializePlayerInputActions();
        InitializeBindingsDictionary();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void InitializeBindingsDictionary()
    {
        bindingsDictionary = new Dictionary<Binding, BindingData>
        {
            { Binding.MoveUp,       new BindingData(PlayerInputActions.Movement.Move, 1)},
            { Binding.MoveDown,     new BindingData(PlayerInputActions.Movement.Move, 2)},
            { Binding.MoveLeft,     new BindingData(PlayerInputActions.Movement.Move, 3)},
            { Binding.MoveRight,    new BindingData(PlayerInputActions.Movement.Move, 4)},

            { Binding.Attack,       new BindingData(PlayerInputActions.Attack.Attack, 0)},

            { Binding.AbilityA,     new BindingData(PlayerInputActions.Abilities.AbilityA, 0)},
            { Binding.AbilityB,     new BindingData(PlayerInputActions.Abilities.AbilityB, 0)},
            { Binding.AbilityC,     new BindingData(PlayerInputActions.Abilities.AbilityC, 0)},

            { Binding.SkipDialogue, new BindingData(PlayerInputActions.Conversations.Skip, 0)},

            { Binding.Stats,        new BindingData(PlayerInputActions.UI.Stats, 0)},
            { Binding.DevMenu,      new BindingData(PlayerInputActions.UI.DevMenu, 0)},
            { Binding.Pause,        new BindingData(PlayerInputActions.UI.Pause, 0)},
        };
    }

    #region Initialization, Enable & Disable
    private void InitializePlayerInputActions()
    {
        PlayerInputActions = new PlayerInputActions();
        LoadInputBindings();
        OnPlayerInputActionsInitialized?.Invoke(this , new OnPlayerInputActionsEventArgs { playerInputActions = PlayerInputActions });
    }

    public void DisableAllActionMaps()
    {
        PlayerInputActions.UI.Disable();
        PlayerInputActions.Movement.Disable();
        PlayerInputActions.Attack.Disable();
        PlayerInputActions.Abilities.Disable();
        PlayerInputActions.Conversations.Disable();
    }

    public void EnableAllActionMaps()
    {
        PlayerInputActions.UI.Enable();
        PlayerInputActions.Movement.Enable();
        PlayerInputActions.Attack.Enable();
        PlayerInputActions.Abilities.Enable();
        PlayerInputActions.Conversations.Enable();
    }
    #endregion

    #region Getters
    public string GetBindingText(Binding binding)
    {
        BindingData bindingData = bindingsDictionary[binding];
        return bindingData.inputAction.bindings[bindingData.bindingIndex].ToDisplayString();
    }

    public InputAction GetBindingInputAction(Binding binding)
    {
        BindingData bindingData = bindingsDictionary[binding];
        return bindingData.inputAction;
    }

    public int GetBindingIndex(Binding binding)
    {
        BindingData bindingData = bindingsDictionary[binding];
        return bindingData.bindingIndex;
    }
    #endregion

    #region Save & Load
    private void LoadInputBindings()
    {
        if (!PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) return;
        PlayerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
    }

    private void SaveInputBindings()
    {
        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, PlayerInputActions.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
    }
    #endregion

    public void RebindBinding(Binding binding)
    {
        OnRebindingStarted?.Invoke(this, new OnRebindingEventArgs { binding = binding });
        DisableAllActionMaps();

        InputAction inputAction = GetBindingInputAction(binding);
        int bindingIndex = GetBindingIndex(binding);

        inputAction.PerformInteractiveRebinding(bindingIndex).
            OnComplete(callback =>
            {
                callback.Dispose();
                EnableAllActionMaps();
                SaveInputBindings();
                OnRebindingCompleted?.Invoke(this, new OnRebindingEventArgs { binding = binding });
            }).Start();
    }
}

