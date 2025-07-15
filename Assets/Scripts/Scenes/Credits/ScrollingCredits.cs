using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingCredits : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform scrollTransform;
    [SerializeField] private UIHoverDetector backToMenuButtonHoverDetector;

    [Header("Settings")]
    [SerializeField, Range(0f, 5f)] private float timeToStartScrolling;
    [SerializeField, Range(0f, 5f)] private float timeToTransitionAfterScrollEnd;

    [SerializeField, Range(20f,50f)] private float baseScrollSpeed = 20f;
    [SerializeField, Range(2f, 15f)] private float scrollSpeedMultiplier = 2f;
    [SerializeField] private float anchoredPositionLimit = 0f;
    [Space]
    [SerializeField] private string endCreditsScene;
    [SerializeField] private TransitionType endCreditsTransitionType;


    private void Start()
    {
        StartCoroutine(ScrollCreditsCoroutine());
    }

    private IEnumerator ScrollCreditsCoroutine()
    {
        yield return new WaitForSeconds(timeToStartScrolling);

        while (!HasReachedLimit())
        {
            bool stall = GetStallHold() && !backToMenuButtonHoverDetector.IsHovering;

            if (stall)
            {
                yield return null;
                continue;
            }

            bool skip = GetSkipHold() && !backToMenuButtonHoverDetector.IsHovering;

            float speed = skip ? baseScrollSpeed * scrollSpeedMultiplier : baseScrollSpeed;
            scrollTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(timeToTransitionAfterScrollEnd);

        ScenesManager.Instance.TransitionLoadTargetScene(endCreditsScene, endCreditsTransitionType);
    }

    private bool GetSkipHold() => Input.GetMouseButton(0);
    private bool GetStallHold() => Input.GetMouseButton(1);
    private bool HasReachedLimit() => scrollTransform.anchoredPosition.y >= anchoredPositionLimit;
}
