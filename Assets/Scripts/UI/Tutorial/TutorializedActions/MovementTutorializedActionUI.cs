using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovementTutorializedActionUI : TutorializedActionUI
{
    [Header("Specific Components")]
    [SerializeField] private TextMeshProUGUI tutorializedActionText;
    [SerializeField] private Image completionBar;

    [Header("Specific Settings")]
    [SerializeField, Range(10f, 50f)] private float distanceCoveredToMetTutorializationCondition;
    [SerializeField, Range(1f, 100f)] private float smoothFillFactor;

    [Header("Runtime Filled")]
    [SerializeField] private float distanceCovered;
    
    private PlayerMovement playerMovement;

    private const float LERP_STOP_THRESHOLD = 0.05f;

    public float DistanceCovered => distanceCovered;

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
        CentralizedInputSystemManager.OnRebindingCompleted += CentralizedInputSystemManager_OnRebindingCompleted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
        CentralizedInputSystemManager.OnRebindingCompleted -= CentralizedInputSystemManager_OnRebindingCompleted;
    }

    protected override void Update()
    {
        base.Update();
        HandleDistanceCoveredUpdate();
        HandleCompletionBar();
    }

    private void HandleDistanceCoveredUpdate()
    {
        if (playerMovement == null) return;

        distanceCovered = playerMovement.DistanceCovered;
    }

    private void HandleCompletionBar()
    {
        if (completionBar.fillAmount >= 1 - LERP_STOP_THRESHOLD) return;
        completionBar.fillAmount = Mathf.Lerp(completionBar.fillAmount, distanceCovered / distanceCoveredToMetTutorializationCondition, smoothFillFactor * Time.deltaTime);
    }

    private void UpdateTutorializedActionText()
    {
        string upBinding = MappingUtilities.MapLongBindingName(CentralizedInputSystemManager.Instance.GetBindingText(Binding.MoveUp));
        string downBinding = MappingUtilities.MapLongBindingName(CentralizedInputSystemManager.Instance.GetBindingText(Binding.MoveDown));
        string leftBinding = MappingUtilities.MapLongBindingName(CentralizedInputSystemManager.Instance.GetBindingText(Binding.MoveLeft));
        string rightBinding = MappingUtilities.MapLongBindingName(CentralizedInputSystemManager.Instance.GetBindingText(Binding.MoveRight));

        tutorializedActionText.text = $"Puedes moverte usando las teclas <b>{upBinding}{leftBinding}{downBinding}{rightBinding}</b>. Prueba moverte un poco";
    }

    #region Virtual Methods
    public override TutorializedAction GetTutorializedAction() => TutorializedAction.Movement;

    protected override bool CheckCondition()
    {
        if (!isDetectingCondition) return false;
        if (distanceCovered >= distanceCoveredToMetTutorializationCondition) return true;
        return false;
    }

    protected override void OpenTutorializedAction()
    {
        completionBar.fillAmount = 0f;
        distanceCovered = 0f;
        UpdateTutorializedActionText();
        base.OpenTutorializedAction();
    }
    #endregion

    #region Subscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerMovement = e.playerTransform.GetComponentInChildren<PlayerMovement>();
    }

    private void CentralizedInputSystemManager_OnRebindingCompleted(object sender, CentralizedInputSystemManager.OnRebindingEventArgs e)
    {
        UpdateTutorializedActionText();
    }
    #endregion
}
