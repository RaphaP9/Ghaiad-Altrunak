using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicsSceneUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button skipCinematicButton;

    private void Awake()
    {
        IntializeButtonsListeners();
    }

    private void IntializeButtonsListeners()
    {
        skipCinematicButton.onClick.AddListener(SkipCinematicScene);
    }

    private void SkipCinematicScene() => CinematicSceneManager.Instance.SkipCinematic();

}
