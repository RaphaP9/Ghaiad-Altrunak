using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsHoverUIContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsHoverUIHandler statsHoverUIHandler;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statDescriptionText;

    private void OnEnable()
    {
        statsHoverUIHandler.OnStatInfoSet += StatsHoverUIHandler_OnStatInfoSet;
    }

    private void OnDisable()
    {
        statsHoverUIHandler.OnStatInfoSet -= StatsHoverUIHandler_OnStatInfoSet;
    }

    private void CompleteSetUI(StatInfoSO statInfo)
    {
        SetStatNameText(statInfo);
        SetStatImage(statInfo);
        SetStatDescriptionText(statInfo);
    }

    private void SetStatNameText(StatInfoSO statInfoSO) => statNameText.text = statInfoSO.statName;
    private void SetStatImage(StatInfoSO statInfoSO) => statImage.sprite = statInfoSO.sprite;
    private void SetStatDescriptionText(StatInfoSO statInfoSO) => statDescriptionText.text = statInfoSO.description;


    private void StatsHoverUIHandler_OnStatInfoSet(object sender, StatsHoverUIHandler.OnStatInfoEventArgs e)
    {
        CompleteSetUI(e.statInfoSO);
    }
}
