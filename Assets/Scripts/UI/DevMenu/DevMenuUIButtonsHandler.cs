using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevMenuUIButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_InputField stageInputField;
    [SerializeField] private TMP_InputField roundInputField;
    [Space]
    [SerializeField] private Button addGoldButton;
    [SerializeField] private Button injectValuesButton;
    [SerializeField] private Button forceJSONSaveButton;
    [SerializeField] private Button reloadSceneButton;

    [Header("Settings")]
    [SerializeField] private int goldToAdd;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private int stageValue = 1;
    private int roundValue = 1;

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

        bool validInputFieldValues = ValidateInputFieldValues();
        if (!validInputFieldValues) return;

        RunDataContainer.Instance.SetCurrentStageNumber(stageValue);
        RunDataContainer.Instance.SetCurrentRoundNumber(roundValue);

        if (debug) Debug.Log($"Injected Values Stage: {stageValue}, Round: {roundValue}.");
    }

    private bool ValidateInputFieldValues()
    {
        bool stageIsInt = int.TryParse(stageInputField.text, out stageValue);
        bool roundIsInt = int.TryParse(roundInputField.text, out roundValue);

        if (!stageIsInt)
        {
            if (debug) Debug.Log($"Cannot parse Stage to Int. Stage InputField Value: {stageInputField.text}");
            return false;
        }

        if (!roundIsInt)
        {
            if (debug) Debug.Log($"Cannot parse Round to Int. Round InputField Value: {roundInputField.text}");
            return false;
        }

        return true;
    }
}
