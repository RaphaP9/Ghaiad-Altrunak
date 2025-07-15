using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKeyUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    private const string PRESS_ANIMATION_NAME = "Press";

    private void OnEnable()
    {
        WelcomeSceneManager.OnAnyKeyPressed += WelcomeSceneManager_OnAnyKeyPressed;
    }

    private void OnDisable()
    {
        WelcomeSceneManager.OnAnyKeyPressed -= WelcomeSceneManager_OnAnyKeyPressed;
    }

    private void WelcomeSceneManager_OnAnyKeyPressed(object sender, System.EventArgs e)
    {
        animator.Play(PRESS_ANIMATION_NAME);
    }
}
