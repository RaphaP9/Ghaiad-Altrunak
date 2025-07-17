using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevMenuUIButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button addGoldButton;
    [SerializeField] private Button injectValuesButton;
    [SerializeField] private Button forceJSONSaveButton;
    [SerializeField] private Button reloadSceneButton;

    [Header("Settings")]
    [SerializeField] private int goldToAdd;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        addGoldButton.onClick.AddListener(AddGold);
        injectValuesButton.onClick.AddListener(InjectValuesToContainer);
        forceJSONSaveButton.onClick.AddListener(ForceJSONSave);
        reloadSceneButton.onClick.AddListener(ReloadScene);
    }

    private void AddGold()
    {
        GoldManager.Instance.AddGoldRaw(goldToAdd);
    }

    private void ReloadScene()
    {
        ScenesManager.Instance.TransitionReloadCurrentScene(TransitionType.Fade);
    }

    private void ForceJSONSave()
    {
        GeneralDataManager.Instance.SaveRunJSONDataAsyncWrapper();
    }

    private void InjectValuesToContainer()
    {
        RunDataContainer.Instance.SetCurrentGold(GoldManager.Instance.CurrentGold);

        if (debug) Debug.Log($"Injected Gold: {GoldManager.Instance.CurrentGold}.");
    }
}
