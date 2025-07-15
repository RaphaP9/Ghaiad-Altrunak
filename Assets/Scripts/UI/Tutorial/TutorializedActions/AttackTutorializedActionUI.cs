using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackTutorializedActionUI : TutorializedActionUI
{
    [Header("Specific Components")]
    [SerializeField] private TextMeshProUGUI tutorializedActionText;
    [SerializeField] private Image completionBar;

    [Header("Specific Settings")]
    [SerializeField, Range(5, 20f)] private int attacksPerformedToMetTutorializationCondition;
    [SerializeField, Range (1f,100f)] private float smoothFillFactor;

    [Header("Runtime Filled")]
    [SerializeField] private int attacksPerformed;

    private const float LERP_STOP_THRESHOLD = 0.05f;

    public int AttacksPerformed => attacksPerformed;

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerAttack.OnAnyPlayerAttack += PlayerAttack_OnAnyPlayerAttack;
        CentralizedInputSystemManager.OnRebindingCompleted += CentralizedInputSystemManager_OnRebindingCompleted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerAttack.OnAnyPlayerAttack -= PlayerAttack_OnAnyPlayerAttack;
        CentralizedInputSystemManager.OnRebindingCompleted -= CentralizedInputSystemManager_OnRebindingCompleted;
    }

    protected override void Update()
    {
        base.Update();
        HandleCompletionBar();
    }

    private void HandleCompletionBar()
    {
        if (completionBar.fillAmount >= 1 - LERP_STOP_THRESHOLD) return;
        completionBar.fillAmount = Mathf.Lerp(completionBar.fillAmount, (float)attacksPerformed / attacksPerformedToMetTutorializationCondition, smoothFillFactor * Time.deltaTime);
    }

    private void UpdateTutorializedActionText()
    {
        string attackBinding = MappingUtilities.MapLongBindingName(CentralizedInputSystemManager.Instance.GetBindingText(Binding.Attack));
        tutorializedActionText.text = $"Puedes atacar usando <b>{attackBinding}</B>. Prueba realizar algunos ataques.";
    }

    private void IncreaseAttacksPerformed(int quantity) => attacksPerformed += quantity;
    public void ResetAttacksPerformed() => attacksPerformed = 0;

    #region Virtual Methods
    public override TutorializedAction GetTutorializedAction() => TutorializedAction.Attack;

    protected override bool CheckCondition()
    {
        if (!isDetectingCondition) return false;
        if (attacksPerformed >= attacksPerformedToMetTutorializationCondition) return true;
        return false;
    }

    protected override void OpenTutorializedAction()
    {
        completionBar.fillAmount = 0f;
        attacksPerformed = 0;
        UpdateTutorializedActionText();
        base.OpenTutorializedAction();
    }
    #endregion


    #region Subscriptions
    private void PlayerAttack_OnAnyPlayerAttack(object sender, PlayerAttack.OnPlayerAttackEventArgs e)
    {
        if (!isDetectingCondition) return;
        IncreaseAttacksPerformed(1);
    }

    private void CentralizedInputSystemManager_OnRebindingCompleted(object sender, CentralizedInputSystemManager.OnRebindingEventArgs e)
    {
        UpdateTutorializedActionText();
    }
    #endregion
}
