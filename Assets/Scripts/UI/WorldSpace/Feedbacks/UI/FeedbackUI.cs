using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeedbackUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected TextMeshProUGUI feedbackText;

    [Header("Settings")]
    [SerializeField, Range(0.1f, 1f)] protected float spawnTime;
    [SerializeField, Range(0.1f, 1f)] protected float showingTime;
    [SerializeField, Range(0.1f, 1f)] protected float hidingTime;
    [Space]
    [SerializeField, Range(0f, 1.5f)] protected float displacementSpeed;
    [SerializeField, Range(0, 360f)] protected float minAngle;
    [SerializeField, Range(0, 360f)] protected float maxAngle;
    [Space]
    [SerializeField] protected bool includePrefix;
    [SerializeField] protected string prefix;
    [Space]
    [SerializeField] protected bool includeSuffix;
    [SerializeField] protected string suffix;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 chosenDirection;

    protected const string SPAWN_ANIMATION_NAME = "Spawn";
    protected const string SHOWING_ANIMATION_NAME = "Showing";
    protected const string HIDING_ANIMATION_NAME = "Hiding";

    protected const string SHOW_TRIGGER = "Show";
    protected const string HIDE_TRIGGER = "Hide";

    protected bool allowDisplacement = false;

    protected void Start()
    {
        StartCoroutine(ShowCoroutine());
        ChooseRandomDirection();
    }

    protected void Update()
    {
        HandleDisplacement();
    }

    protected IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(spawnTime);

        animator.SetTrigger(SHOW_TRIGGER);
        yield return new WaitForSeconds(showingTime);

        allowDisplacement = true;

        animator.SetTrigger(HIDE_TRIGGER);
        yield return new WaitForSeconds(hidingTime);

        Destroy(transform.root.gameObject);
    }

    protected void HandleDisplacement()
    {
        if (!allowDisplacement) return;
        transform.position += displacementSpeed * Time.deltaTime * GeneralUtilities.Vector2ToVector3(chosenDirection);
    }

    protected void PlayAnimationWithName(string animationName)
    {
        animator.Play(animationName);
    }

    protected void SetTextColor(Color color) => feedbackText.color = color;

    protected void SetText(string text)
    {
        text = includePrefix ? prefix + text : text;
        text = includeSuffix ? text + suffix : text;
        feedbackText.text = text;
    }

    public virtual void SetFeedback(Color textColor)
    {
        SetTextColor(textColor);
    }

    protected void AugmentTextScale(float scale) => feedbackText.transform.localScale = feedbackText.transform.localScale * scale;

    protected void ChooseRandomDirection()
    {
        float randomAngle = Random.Range(minAngle, maxAngle);
        Vector2 randomDirection = GeneralUtilities.AngleDegreesToVector2(randomAngle);

        chosenDirection = randomDirection;
    }
}
